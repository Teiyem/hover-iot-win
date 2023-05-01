using System.Windows;
using System.Windows.Controls;

namespace Hover.AttachedProperties;

#region Attribution
// Code attribution: Original Author Luke Malpass (Angel Six)
// Note: Few changes made
#endregion

/// <summary>
/// The MonitorPassword attached property for a <see cref="PasswordBox"/>.
/// </summary>
public class MonitorPasswordProperty : BaseAttachedProperty<MonitorPasswordProperty, bool>
{
    public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        if (sender is not PasswordBox passwordBox)
            return;

        passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;

        if (!(bool) e.NewValue) return;

        HasTextProperty.SetValue(passwordBox);

        passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
    }

    /// <summary>
    /// Fired when the password box password value changes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        HasTextProperty.SetValue((PasswordBox)sender);
    }
}

/// <summary>
/// The HasText attached property for a <see cref="PasswordBox"/>.
/// </summary>
public class HasTextProperty : BaseAttachedProperty<HasTextProperty, bool>
{
    /// <summary>
    /// Sets the HasText property based on if the caller <see cref="PasswordBox"/> has any text.
    /// </summary>
    /// <param name="sender"></param>
    public static void SetValue(DependencyObject sender)
    {
        SetValue(sender, ((PasswordBox)sender).SecurePassword.Length > 0);
    }
}