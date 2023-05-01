using System;
using System.Globalization;

namespace Hover.Converters;

/// <summary>
/// A converter that takes in a boolean and inverts it
/// </summary>
public class InvertBoolConverter : BaseConverter<InvertBoolConverter>
{
    /// <summary>
    /// Conversion method
    /// </summary>
    /// <param name="value"> value being converted</param>
    /// <param name="targetType"> generic type </param>
    /// <param name="parameter"> option conversion param </param>
    /// <param name="culture"> conversion culture </param>
    /// <returns> inverted boolean </returns>
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value != null && !(bool)value;
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
        throw new NotSupportedException();
    }
}