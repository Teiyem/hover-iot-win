using System.Security;
using System.Threading.Tasks;
using System.Windows.Input;
using Hover.Commands;
using Hover.Models;

namespace Hover.ViewModels;

/// <summary>
/// The View Model for a register page
/// </summary>
public class RegisterViewModel : BaseViewModel
{
    /// <summary>
    /// The application view model.
    /// </summary>
    private readonly ApplicationViewModel mApplication;

    /// <summary>
    /// Gets or sets the name of the user.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the username of the user.
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the register command is running.
    /// </summary>
    public bool RegisterIsRunning { get; set; }

    /// <summary>
    /// Gets or sets the command to login.
    /// </summary>
    public ICommand Login { get; set; }

    /// <summary>
    /// Gets or sets the command to register for a new account.
    /// </summary>
    public ICommand Register { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Register"/> class.
    /// </summary>
    public RegisterViewModel()
    {
        mApplication = ApplicationViewModel.GetInstance();
        Register = new RelayParameterizedCommand(async (parameter) => await RegisterAsync(parameter));
        Login = new RelayCommand(async () => await LoginAsync());
    }

    /// <summary>
    /// Attempts to register a new user.
    /// </summary>
    /// <param name="parameter">The <see cref="SecureString"/> passed in from the view for the users password</param>
    /// <returns></returns>
    public async Task RegisterAsync(object parameter)
    {
        await RunCommandAsync(() => RegisterIsRunning, async () =>
        {
            // var user = new User
            // {
            //     Name = Name,
            //     Username = Username,
            //     Password = (parameter as IHavePassword)?.SecurePassword.Unsecure()
            // };

            await LoginAsync();
        });
    }

    /// <summary>
    /// Takes the user to the login page.
    /// </summary>
    /// <returns></returns>
    public async Task LoginAsync()
    {
        mApplication.GoToPage(AppPage.Login);

        await Task.Delay(1);
    }
}