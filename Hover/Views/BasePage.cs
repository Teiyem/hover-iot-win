using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Hover.Animation;
using Hover.Models;

namespace Hover.Views;

#region Attribution
// Code attribution: Original Author Luke Malpass (Angel Six)
// Note: Changes made.
// OnBindDataContext & OnGetViewModel Isn't part of the attribution
#endregion

/// <summary>
/// The base page for all pages to gain base functionality.
/// </summary>
public class BasePage : UserControl
{
    /// <summary>
    /// The animation the play when the page is first loaded.
    /// </summary>
    public PageAnimation PageLoadAnimation { get; set; } = PageAnimation.SlideAndFadeInFromRight;

    /// <summary>
    /// The animation the play when the page is unloaded.
    /// </summary>
    public PageAnimation PageUnloadAnimation { get; set; } = PageAnimation.SlideAndFadeOutToLeft;

    /// <summary>
    /// The time any slide animation takes to complete.
    /// </summary>
    public float SlideSeconds { get; set; } = 0.4f;

    /// <summary>
    /// A flag to indicate if this page should animate out on load.
    /// Useful for when we are moving the page to another frame.
    /// </summary>
    public bool ShouldAnimateOut { get; set; }

    /// <summary>
    /// Default constructor.
    /// </summary>
    public BasePage()
    {
        if (DesignerProperties.GetIsInDesignMode(this))
            return;

        if (PageLoadAnimation != PageAnimation.None)
            Visibility = Visibility.Collapsed;

        Initialized += (s,e) => OnBindDataContext(s as DependencyObject);

        Loaded += OnPageLoadedAsync;
    }

    /// <summary>
    /// Once the page is loaded, perform any required animation
    /// </summary>
    /// <param name="sender"> The page </param>
    /// <param name="e"></param>
    private async void OnPageLoadedAsync(object sender, RoutedEventArgs e)
    {
        if (ShouldAnimateOut)
            await AnimateOutAsync();
        else
            await AnimateInAsync();
    }

    /// <summary>
    /// Animates the page in.
    /// </summary>
    /// <returns></returns>
    public async Task AnimateInAsync()
    {
        if (PageLoadAnimation == PageAnimation.None)
            return;

        switch (PageLoadAnimation)
        {
            case PageAnimation.SlideAndFadeInFromRight:
                await this.SlideAndFadeInAsync(AnimationDirection.Right, false, SlideSeconds, size: (int)Application.Current.MainWindow!.Width);
                break;
        }
    }

    /// <summary>
    /// Animates the page out.
    /// </summary>
    /// <returns></returns>
    public async Task AnimateOutAsync()
    {
        if (PageUnloadAnimation == PageAnimation.None)
            return;

        switch (PageUnloadAnimation)
        {
            case PageAnimation.SlideAndFadeOutToLeft:
                await this.SlideAndFadeOutAsync(AnimationDirection.Left, SlideSeconds);
                break;
        }
    }

    /// <summary>
    /// Automatically sets the data context of the page.
    /// </summary>
    /// <param name="page"> The page to set.</param>
    private static void OnBindDataContext(DependencyObject page)
    {
        if (page is not FrameworkElement element) return;
        var viewModel = FindViewModelTypeForPageType(element.GetType());
        if(viewModel != null)
            element.DataContext = Activator.CreateInstance(viewModel);
    }

    /// <summary>
    /// Finds the page model based on the naming conversion 
    /// </summary>
    /// <param name="pageType"> the page </param>
    /// <returns> The page model. </returns>
    private static Type FindViewModelTypeForPageType(Type pageType)
    {
        var viewName = "";
            
        if(pageType.FullName != null && pageType.FullName.EndsWith("Page", StringComparison.InvariantCulture))
        {
            viewName = pageType.FullName?.Replace("Page", string.Empty, StringComparison.InvariantCulture)
                .Replace("Views", "ViewModels", StringComparison.InvariantCulture);
        }

        var fullName = pageType.GetTypeInfo().Assembly.FullName;
        var typeName = $"{viewName}ViewModel, {fullName}";

        return Type.GetType(typeName);
    }
}