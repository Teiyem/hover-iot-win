using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Hover.Converters;

#region Attribution
// Code attribution: Original Author Luke Malpass (Angel Six)
#endregion

/// <summary>
/// Base class for value converters used in WPF applications.
/// </summary>
/// <typeparam name="T">The derived class that implements this base class.</typeparam>
public abstract class BaseConverter<T> : MarkupExtension, IValueConverter
    where T : class, new()
{
    /// <summary>
    /// Holds a single instance of the converter.
    /// </summary>
    private static T _converter;

    /// <summary>
    /// Provides a value for the XAML markup extension.
    /// </summary>
    /// <param name="serviceProvider">A service provider that can provide services for the markup extension.</param>
    /// <returns>The instance of the derived class.</returns>
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return _converter ??= new T();
    }

    /// <summary>
    /// Converts a value from the source type to the target type.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="targetType">The target type to convert to.</param>
    /// <param name="parameter">An optional parameter to use in the conversion.</param>
    /// <param name="culture">The culture information to use in the conversion.</param>
    /// <returns>The converted value.</returns>
    public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

    /// <summary>
    /// Converts a value from the target type back to the source type.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="targetType">The target type to convert to.</param>
    /// <param name="parameter">An optional parameter to use in the conversion.</param>
    /// <param name="culture">The culture information to use in the conversion.</param>
    /// <returns>The converted value.</returns>
    public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);

}