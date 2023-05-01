using System;
using System.Globalization;
using Hover.Models;
using Hover.Views;

namespace Hover.Converters;

#region Attribution
// Code attribution: Original Author Luke Malpass (Angel Six)
// Note: Few changes made
#endregion

/// <summary>
/// Converts the <see cref="AppPage"/> to it's associated view/page
/// </summary>
public class PageConverter : BaseConverter<PageConverter>
{
    /// <summary>
    /// Conversion method
    /// </summary>
    /// <param name="value"> value being converted</param>
    /// <param name="targetType"> generic type </param>
    /// <param name="parameter"> option conversion param </param>
    /// <param name="culture"> conversion culture </param>
    /// <returns> an app page</returns>
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null) return null;
        return (AppPage) value switch
        {
            AppPage.Home => new HomePage(),
            AppPage.Login => new LoginPage(),
            AppPage.Register => new RegisterPage(),
            _ => null
        };
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