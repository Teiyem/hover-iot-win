using System.Windows;
using System.Windows.Controls;

namespace Hover.AttachedProperties;

#region Attribution
// Code attribution: Original Author Luke Malpass (Angel Six)
#endregion

/// <summary>
/// The NoFrameHistory attached property for creating a <see cref="Frame"/>
/// that never shows navigation and keeps the navigation history empty
/// </summary>
public class NoFrameHistory : BaseAttachedProperty<NoFrameHistory, bool>
{
    /// <summary>
    /// Gets the Frame, hides the navigation bar and hooks in on the
    /// navigated event in order to clear the history 
    /// </summary>
    /// <param name="sender">The frame</param>
    /// <param name="e">The arguments for the event</param>
    public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        if (sender is not Frame frame) return;
        frame.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
        frame.Navigated += (ss, ee) => ((Frame) ss).NavigationService.RemoveBackEntry();
    }
}