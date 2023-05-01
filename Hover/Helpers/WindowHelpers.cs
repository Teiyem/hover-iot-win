using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using Hover.Models;

namespace Hover.Helpers;

#region Attribution
// Code attribution: Original Author Luke Malpass (Angel Six)
#endregion

/// <summary>
/// Fixes the issue with Windows of Style <see cref="WindowStyle.None"/>
/// </summary>
public class WindowHelpers
{
    /// <summary>
    /// The window to handle the resizing for
    /// </summary>
    private readonly Window mWindow;

    /// <summary>
    /// The last calculated available screen size
    /// </summary>
    private Rect mScreenSize;

    /// <summary>
    /// How close to the edge the window has to be to be detected as at the edge of the screen
    /// </summary>
    private const int EdgeTolerance = 1;

    /// <summary>
    /// The transform matrix used to convert WPF sizes to screen pixels
    /// </summary>
    private DpiScale? mMonitorDpi;

    /// <summary>
    /// The last known dock position
    /// </summary>
    private DockPosition mLastDock = DockPosition.UnDocked;

    /// <summary>
    /// A flag indicating if the window is currently being moved/dragged
    /// </summary>
    private bool mBeingMoved;

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetCursorPos(out POINT lpPoint);

    [DllImport("user32.dll")]
    private static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr MonitorFromPoint(POINT pt, MonitorOptions dwFlags);

    [DllImport("user32.dll")]
    private static extern IntPtr MonitorFromWindow(IntPtr hwnd, MonitorOptions dwFlags);

    /// <summary>
    /// Called when the window dock position changes
    /// </summary>
    public event Action<DockPosition> WindowDockChanged = (dock) => { };

    /// <summary>
    /// Called when the window starts being moved/dragged
    /// </summary>
    public event Action WindowStartedMove = () => { };

    /// <summary>
    /// Called when the window has been moved/dragged and then finished
    /// </summary>
    public event Action WindowFinishedMove = () => { };

    /// <summary>
    /// The size and position of the current monitor the window is on
    /// </summary>
    public Rectangle CurrentMonitorSize { get; set; }

    /// <summary>
    /// The margin around the window for the current window to compensate for any non-usable area
    /// such as the task bar
    /// </summary>
    public Thickness CurrentMonitorMargin { get; private set; }

    /// <summary>
    /// The size and position of the current screen in relation to the multi-screen desktop
    /// For example a second monitor on the right will have a Left position of
    /// the X resolution of the screens on the left
    /// </summary>
    public Rect CurrentScreenSize => mScreenSize;

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="window">The window to monitor and correctly maximize</param>
    public WindowHelpers(Window window)
    {
        mWindow = window;
        mWindow.SourceInitialized += Window_SourceInitialized;
        mWindow.SizeChanged += Window_SizeChanged;
        mWindow.LocationChanged += Window_LocationChanged;
    }

    /// <summary>
    /// Initialize and hook into the windows message pump
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Window_SourceInitialized(object sender, EventArgs e)
    {
        var handle = (new WindowInteropHelper(mWindow)).Handle;
        var handleSource = HwndSource.FromHwnd(handle);
        handleSource?.AddHook(WindowProc);
    }

    /// <summary>
    /// Monitor for moving of the window and constantly check for docked positions
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Window_LocationChanged(object sender, EventArgs e)
    {
        Window_SizeChanged(null, null);
    }

    /// <summary>
    /// Monitors for size changes and detects if the window has been docked (Aero snap) to an edge
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        WmGetMinMaxInfo(IntPtr.Zero, IntPtr.Zero);
        mMonitorDpi = VisualTreeHelper.GetDpi(mWindow);
        if (mMonitorDpi == null)
            return;

        var top = mWindow.Top;
        var left = mWindow.Left;
        var bottom = top + mWindow.Height;
        var right = left + mWindow.Width;

        var windowTopLeft = new Point(left * mMonitorDpi.Value.DpiScaleX, top * mMonitorDpi.Value.DpiScaleX);
        var windowBottomRight = new Point(right * mMonitorDpi.Value.DpiScaleX, bottom * mMonitorDpi.Value.DpiScaleX);

        var edgedTop = windowTopLeft.Y <= (mScreenSize.Top + EdgeTolerance) && windowTopLeft.Y >= (mScreenSize.Top - EdgeTolerance);
        var edgedLeft = windowTopLeft.X <= (mScreenSize.Left + EdgeTolerance) && windowTopLeft.X >= (mScreenSize.Left - EdgeTolerance);
        var edgedBottom = windowBottomRight.Y >= (mScreenSize.Bottom - EdgeTolerance) && windowBottomRight.Y <= (mScreenSize.Bottom + EdgeTolerance);
        var edgedRight = windowBottomRight.X >= (mScreenSize.Right - EdgeTolerance) && windowBottomRight.X <= (mScreenSize.Right + EdgeTolerance);

        DockPosition dock;

        if (edgedTop && edgedBottom && edgedLeft)
            dock = DockPosition.Left;

        else if (edgedTop && edgedBottom && edgedRight)
            dock = DockPosition.Right;
        
        else if (edgedTop && edgedBottom)
            dock = DockPosition.TopBottom;
    
        else if (edgedTop && edgedLeft)
            dock = DockPosition.TopLeft;

        else if (edgedTop && edgedRight)
            dock = DockPosition.TopRight;

        else if (edgedBottom && edgedLeft)
            dock = DockPosition.BottomLeft;

        else if (edgedBottom && edgedRight)
            dock = DockPosition.BottomRight;

        else
            dock = DockPosition.UnDocked;

        if (dock != mLastDock)
            WindowDockChanged(dock);

        mLastDock = dock;
    }

    /// <summary>
    /// Listens out for all windows messages for this window
    /// </summary>
    /// <param name="hwnd"></param>
    /// <param name="msg"></param>
    /// <param name="wParam"></param>
    /// <param name="lParam"></param>
    /// <param name="handled"></param>
    /// <returns></returns>
    private IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        switch (msg)
        {
            // Handle the GetMinMaxInfo of the Window
            case 0x0024: 
                WmGetMinMaxInfo(hwnd, lParam);
                handled = true;
                break;

            // Once the window starts being moved
            case 0x0231:
                mBeingMoved = true;
                WindowStartedMove();
                break;

            // Once the window has finished being moved
            case 0x0232:
                mBeingMoved = false;
                WindowFinishedMove();
                break;
        }
        return (IntPtr)0;
    }

    /// <summary>
    /// Get the min/max window size for this window
    /// Correctly accounting for the task bar size and position
    /// </summary>
    /// <param name="hwnd"></param>
    /// <param name="lParam"></param>
    private void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
    {
        GetCursorPos(out var lMousePosition);

        var lCurrentScreen = mBeingMoved ?
            MonitorFromPoint(lMousePosition, MonitorOptions.MONITOR_DEFAULTTONULL) : 
            MonitorFromWindow(hwnd, MonitorOptions.MONITOR_DEFAULTTONULL);

        var lPrimaryScreen = MonitorFromPoint(new POINT(0,0), MonitorOptions.MONITOR_DEFAULTTOPRIMARY);

        var lCurrentScreenInfo = new MONITORINFO();
        if (GetMonitorInfo(lCurrentScreen, lCurrentScreenInfo) == false)
            return;

        var lPrimaryScreenInfo = new MONITORINFO();
        if (GetMonitorInfo(lPrimaryScreen, lPrimaryScreenInfo) == false)
            return;

        mMonitorDpi = VisualTreeHelper.GetDpi(mWindow);

        var currentX = lCurrentScreenInfo.RCWork.Left - lCurrentScreenInfo.RCMonitor.Left;
        var currentY = lCurrentScreenInfo.RCWork.Top - lCurrentScreenInfo.RCMonitor.Top;
        var currentWidth = (lCurrentScreenInfo.RCWork.Right - lCurrentScreenInfo.RCWork.Left);
        var currentHeight = (lCurrentScreenInfo.RCWork.Bottom - lCurrentScreenInfo.RCWork.Top);

        if (lParam != IntPtr.Zero)
        {
            var lMmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO))!;

            lMmi.PointMaxPosition.X = lPrimaryScreenInfo.RCMonitor.Left;
            lMmi.PointMaxPosition.Y = lPrimaryScreenInfo.RCMonitor.Top;
            lMmi.PointMaxSize.X = lPrimaryScreenInfo.RCMonitor.Right;
            lMmi.PointMaxSize.Y = lPrimaryScreenInfo.RCMonitor.Bottom;

            var minSize = new Point(mWindow.MinWidth * mMonitorDpi.Value.DpiScaleX, mWindow.MinHeight * mMonitorDpi.Value.DpiScaleX);
            lMmi.PointMinTrackSize.X = (int)minSize.X;
            lMmi.PointMinTrackSize.Y = (int)minSize.Y;

            Marshal.StructureToPtr(lMmi, lParam, true);
        }

        CurrentMonitorSize = new Rectangle(currentX, currentY, currentWidth + currentX, currentHeight + currentY);

        CurrentMonitorMargin = new Thickness(
            (lCurrentScreenInfo.RCWork.Left - lCurrentScreenInfo.RCMonitor.Left) / mMonitorDpi.Value.DpiScaleX,
            (lCurrentScreenInfo.RCWork.Top - lCurrentScreenInfo.RCMonitor.Top) / mMonitorDpi.Value.DpiScaleY,
            (lCurrentScreenInfo.RCMonitor.Right - lCurrentScreenInfo.RCWork.Right) / mMonitorDpi.Value.DpiScaleX,
            (lCurrentScreenInfo.RCMonitor.Bottom - lCurrentScreenInfo.RCWork.Bottom) / mMonitorDpi.Value.DpiScaleY
        );

        mScreenSize = new Rect(lCurrentScreenInfo.RCWork.Left, lCurrentScreenInfo.RCWork.Top, currentWidth, currentHeight);
    }

    /// <summary>
    /// Gets the current cursor position in screen coordinates relative to an entire multi-desktop position
    /// </summary>
    /// <returns></returns>
    public Point GetCursorPosition()
    {
        GetCursorPos(out var lMousePosition);

        if (mMonitorDpi == null) return new Point();

        return new Point(lMousePosition.X / mMonitorDpi.Value.DpiScaleX, 
            lMousePosition.Y / mMonitorDpi.Value.DpiScaleY);
    }
}

public enum MonitorOptions : uint
{
    MONITOR_DEFAULTTONULL = 0x00000000,
    MONITOR_DEFAULTTOPRIMARY = 0x00000001,
    MONITOR_DEFAULTTONEAREST = 0x00000002
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public class MONITORINFO
{
    internal int CBSize = Marshal.SizeOf(typeof(MONITORINFO));
    internal Rectangle RCMonitor;
    internal Rectangle RCWork;
    internal int DWFlags;
}

[StructLayout(LayoutKind.Sequential)]
public struct Rectangle
{
    internal int Left, Top, Right, Bottom;

    public Rectangle(int left, int top, int right, int bottom)
    {
        Left = left;
        Top = top;
        Right = right;
        Bottom = bottom;
    }
}

[StructLayout(LayoutKind.Sequential)]
public struct MINMAXINFO
{
    internal POINT PointReserved; 
    internal POINT PointMaxSize;
    internal POINT PointMaxPosition;
    internal POINT PointMinTrackSize;
    internal POINT PointMaxTrackSize;
};

[StructLayout(LayoutKind.Sequential)]
public struct POINT
{
    /// <summary>
    /// x coordinate of point.
    /// </summary>
    internal int X;
    /// <summary>
    /// y coordinate of point.
    /// </summary>
    internal int Y;

    /// <summary>
    /// Construct a point of coordinates (x,y).
    /// </summary>
    public POINT(int x, int y)
    {
        X = x;
        Y = y;
    }

    public override string ToString()
    {
        return $"{X} {Y}";
    }
}