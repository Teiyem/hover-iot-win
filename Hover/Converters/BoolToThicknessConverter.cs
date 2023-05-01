using System;
using System.Globalization;

namespace Hover.Converters;

#region Attribution
// Code attribution: Original Author Luke Malpass (Angel Six)
// Note: Few changes made
#endregion

/// <summary>
/// A converter that takes in a boolean and returns a thickness of 2 if true
/// </summary>
public class BoolToThicknessConverter : BaseConverter<BoolToThicknessConverter>
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
            return value != null && (bool)value ? 1 : 0;
        return value != null && (bool)value ? 0 : 1;
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