using Hover.Views;

namespace Hover.ViewModels;

/// <summary>
/// A viewmodel for the <see cref="HomePage"/>.
/// </summary>
public class HomeViewModel : BaseViewModel
{
    /// <summary>
    /// The application view model.
    /// </summary>
    private readonly ApplicationViewModel mApplication;

    /// <summary>
    /// Initializes a new instance of the <see cref="HomeViewModel"/> class.
    /// </summary>
    public HomeViewModel()
    {
        mApplication = ApplicationViewModel.GetInstance();
    }
}