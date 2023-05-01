using System.Security;
using System.Threading.Tasks;
using System.Windows.Input;
using Hover.Commands;
using Hover.Models;

namespace Hover.ViewModels;

/// <summary>
/// The View Model for the login page
/// </summary>
public class LoginViewModel : BaseViewModel
{
    /// <summary>
    /// The application view model.
    /// </summary>
    private readonly ApplicationViewModel mApplication;

    /// <summary>
    /// Gets or sets the user's email.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the login command is running.
    /// </summary>
    public bool LoginIsRunning { get; set; }

    /// <summary>
    /// Gets or sets the command to login.
    /// </summary>
    public ICommand Login { get; set; }

    /// <summary>
    /// Gets or set the command to register to go to the registration page.
    /// </summary>
    public ICommand Register { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="LoginViewModel"/> class.
    /// </summary>
    public LoginViewModel()
    {
        mApplication = ApplicationViewModel.GetInstance();
        Login = new RelayParameterizedCommand(async (parameter) => await LoginAsync(parameter));
        Register = new RelayCommand(async () => await RegisterAsync());
    }

    /// <summary>
    /// Attempts to log the user in.
    /// </summary>
    /// <param name="parameter">The <see cref="SecureString"/> passed in from the view for the users password</param>
    /// <returns></returns>
    public async Task LoginAsync(object parameter)
    {
        await RunCommandAsync(() => LoginIsRunning, async () =>
        {
            await mApplication.HandleSuccessfulLogin();
        });
    }

    /// <summary>
    /// Takes the user to the register page.
    /// </summary>
    /// <returns></returns>
    public async Task RegisterAsync()
    {
        mApplication.GoToPage(AppPage.Register);

        await Task.Delay(1);
    }
}