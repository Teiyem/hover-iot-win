using System;
using Hover.Commands;
using Hover.Helpers;
using Hover.Models;
using System.Windows.Input;
using System.Windows;

namespace Hover.ViewModels;

#region Attribution
// Code attribution: Original Author Luke Malpass (Angel Six)
// Note: Changes made
#endregion

/// <summary>
/// The View Model for the custom window.
/// </summary>
public class MainViewModel : BaseViewModel
{
    /// <summary>
    /// The window this view model controls.
    /// </summary>
    private readonly Window mWindow;

    /// <summary>
    /// The window helper that keeps the window size correct in various states.
    /// </summary>
    private readonly WindowHelpers mHelper;

    /// <summary>
    /// The last known dock position.
    /// </summary>
    private DockPosition mDockPosition = DockPosition.UnDocked;

    /// <summary>
    /// The margin around the window to allow for a drop shadow.
    /// </summary>
    private Thickness mOuterMarginSize = new(5);

    /// <summary>
    /// Gets or sets a value indicating whether the window is currently being moved/dragged.
    /// </summary>
    public bool BeingMoved { get; set; }

    /// <summary>
    /// The corner radius of the window.
    /// </summary>
    private int mWindowRadius = 11;

    /// <summary>
    /// Gets or sets a value indicating whether a dimmed overlay is visible on the window.
    /// </summary>
    public bool DimOverlayVisible { get; set; }

    /// <summary>
    /// True if the window should be borderless because it is docked or maximized.
    /// </summary>
    public bool Borderless => (mWindow.WindowState == WindowState.Maximized || mDockPosition != DockPosition.UnDocked);

    /// <summary>
    /// The size of the resize border around the window.
    /// </summary>
    public int ResizeBorder => mWindow.WindowState == WindowState.Maximized ? 0 : 4;

    /// <summary>
    /// The margin around the window to allow for a drop shadow.
    /// </summary>
    public Thickness OuterMarginSize
    {
        get => mWindow.WindowState == WindowState.Maximized ? mHelper.CurrentMonitorMargin : (Borderless ? new Thickness(0) : mOuterMarginSize);
        set => mOuterMarginSize = value;
    }

    /// <summary>
    /// The size of the resize border around the window,
    /// taking into account the outer margin.
    /// </summary>
    public Thickness ResizeBorderThickness
        => new(OuterMarginSize.Left + ResizeBorder,
            OuterMarginSize.Top + ResizeBorder, OuterMarginSize.Right + ResizeBorder,
            OuterMarginSize.Bottom + ResizeBorder);

    /// <summary>
    /// The radius of the edges of the window.
    /// </summary>
    public int WindowRadius
    {
        get => Borderless ? 0 : mWindowRadius;
        set => mWindowRadius = value;
    }

    /// <summary>
    /// The rectangle border around the window when docked.
    /// </summary>
    public int FlatBorderThickness => Borderless && mWindow.WindowState != WindowState.Maximized ? 1 : 0;

    /// <summary>
    /// The radius of the edges of the window.
    /// </summary>
    public CornerRadius WindowCornerRadius => new(WindowRadius);

    /// <summary>
    /// Gets or sets the height of the title bar / caption of the window.
    /// </summary>
    public int TitleHeight { get; set; } = 30;

    /// <summary>s
    /// The height of the title bar / caption of the window.
    /// </summary>
    public GridLength TitleHeightGridLength => new(TitleHeight + ResizeBorder);

    /// <summary>
    /// Gets or the Application viewmodel.
    /// </summary>
    public ApplicationViewModel ApplicationVm { get; }

    /// <summary>
    /// Gets the command to minimize the window.
    /// </summary>
    public ICommand Minimize { get; }

    /// <summary>
    /// Gets the command to maximize the window.
    /// </summary>
    public ICommand Maximize { get; }

    /// <summary>
    /// Gets the command to close the window.
    /// </summary>
    public ICommand Close { get; }

    /// <summary>
    /// Gets the command to show the system menu of the window.
    /// </summary>
    public ICommand Menu { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MainViewModel"/> class.
    /// </summary>
    public MainViewModel(Window window)
    {
        ApplicationVm = ApplicationViewModel.GetInstance();

        mWindow = window;

        mWindow.StateChanged += (_, _) => { OnWindowResized(); };

        mWindow.Activated += (_, _) => { DimOverlayVisible = false; };

        mWindow.Deactivated += (_, _) => { DimOverlayVisible = true; };

        mHelper = new WindowHelpers(mWindow);

        mHelper.WindowDockChanged += (dock) =>
        {
            mDockPosition = dock;
            OnWindowResized();
        };

        mHelper.WindowStartedMove += () => { BeingMoved = true; };

        mHelper.WindowFinishedMove += () =>
        {
            BeingMoved = false;
            if (mDockPosition == DockPosition.UnDocked &&
                Math.Abs(mWindow.Top - mHelper.CurrentScreenSize.Top) < 0.0001)
                mWindow.Top = -OuterMarginSize.Top;
        };

        Minimize = new RelayCommand(() => mWindow.WindowState = WindowState.Minimized);
        Maximize = new RelayCommand(() => mWindow.WindowState ^= WindowState.Maximized);
        Close = new RelayCommand(() => mWindow.Close());
        Menu = new RelayCommand(() => SystemCommands.ShowSystemMenu(mWindow, GetMousePosition()));
    }

    /// <summary>
    /// Gets the position of the mouse relative to the window.
    /// </summary>
    /// <returns>The mouse position.</returns>
    private Point GetMousePosition()
    {
        return mHelper.GetCursorPosition();
    }

    /// <summary>
    /// If the window resizes to a special position (docked or maximized)
    /// this will update all required property change events to set the borders and radius values.
    /// </summary>
    private void OnWindowResized()
    {
        OnPropertyChanged(nameof(Borderless));
        OnPropertyChanged(nameof(FlatBorderThickness));
        OnPropertyChanged(nameof(ResizeBorderThickness));
        OnPropertyChanged(nameof(OuterMarginSize));
        OnPropertyChanged(nameof(WindowRadius));
        OnPropertyChanged(nameof(WindowCornerRadius));
    }

    /// <summary>
    /// Navigates to another page.
    /// </summary>
    /// <param name="page">The page to go to.</param>
    private void Navigate(AppPage page)
    {
        ApplicationVm.GoToPage(page);
    }
}