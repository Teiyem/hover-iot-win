using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Hover.AttachedProperties;

#region Attribution
// Code attribution: Original Author Luke Malpass (Angel Six)
#endregion

/// <summary>
/// Focuses (keyboard focus) this element on load
/// </summary>
public class IsFocusedProperty : BaseAttachedProperty<IsFocusedProperty, bool>
{
    public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        // If we don't have a control, return
        if (sender is not Control control)
            return;

        // Focus this control once loaded
        control.Loaded += (s, se) => control.Focus();
    }
}

/// <summary>
/// Focuses (keyboard focus) this element if true
/// </summary>
public class FocusProperty : BaseAttachedProperty<FocusProperty, bool>
{
    public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        if (sender is not Control control)
            return;

        if ((bool)e.NewValue)
            control.Focus();
    }
}

/// <summary>
/// Focuses (keyboard focus) and selects all text in this element if true
/// </summary>
public class FocusAndSelectProperty : BaseAttachedProperty<FocusAndSelectProperty, bool>
{
    public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        if (sender is not TextBoxBase control) return;
        if (!(bool) e.NewValue) return;
        control.Focus();
        control.SelectAll();
    }
}