using System.Threading.Tasks;
using Hover.Models;

namespace Hover.ViewModels;

/// <summary>
/// The application state as a view model.
/// </summary>
public class ApplicationViewModel : BaseViewModel
{
    /// <summary>
    /// The singleton instance of <see cref="ApplicationViewModel"/> class.
    /// </summary>
    private static readonly ApplicationViewModel mInstance = new();

    /// <summary>
    /// Gets or sets a value indicating whether the navigator is visible or not.
    /// </summary>
    public bool NavigatorVisible { get; set; }

    /// <summary>
    /// Gets the current page of the application. The default is the home page.
    /// </summary>
    public AppPage CurrentPage { get; private set; } = AppPage.Login;

    /// <summary>
    /// Gets the singleton instance of the <see cref="ApplicationViewModel"/> class.
    /// </summary>
    /// <returns>The instance of the <see cref="ApplicationViewModel"/> class.</returns>
    public static ApplicationViewModel GetInstance()
    {
        return mInstance;
    }

    /// <summary>
    /// Navigates to the specified page
    /// </summary>
    /// <param name="page"> the page to go to</param>
    public void GoToPage(AppPage page)
    {
        NavigatorVisible = page != AppPage.Login && page != AppPage.Register;

        CurrentPage = page;
    }

    /// <summary>
    /// Handles what happens when we have successfully logged in.
    /// </summary>
    public async Task HandleSuccessfulLogin()
    {
        await Task.Delay(10);
        GoToPage(AppPage.Home);
    }
}