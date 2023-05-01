using System;
using System.Globalization;
using System.Windows;

namespace Hover.Converters;

/// <summary>
/// A converter that takes in a boolean and returns a <see cref="Visibility"/>
/// </summary>
public class BooleanToVisibilityConverter : BaseConverter<BooleanToVisibilityConverter>
{
    /// <summary>
    /// Conversion method
    /// </summary>
    /// <param name="value"> value being converted</param>
    /// <param name="targetType"> generic type </param>
    /// <param name="parameter"> option conversion param </param>
    /// <param name="culture"> conversion culture </param>
    /// <returns> a thickness </returns>
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (parameter == null)
            return value != null && (bool)value ? Visibility.Hidden : Visibility.Visible;
        return value != null && (bool)value ? Visibility.Visible : Visibility.Hidden;
    }

    /// <summary>
    /// Revert back conversion method which is not desired this application
    /// </summary>
    /// <param name="value"> value being converted</param>
    /// <param name="targetType"> generic type </param>
    /// <param name="parameter"> option conversion param </param>
    /// <param name="culture"> conversion culture </param>
    /// <returns> exception as we don't convert back </returns>
    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}