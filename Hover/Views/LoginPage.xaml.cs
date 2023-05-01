using System.Security;
using Hover.Models;

namespace Hover.Views;

/// <summary>
/// The login page.
/// </summary>
public partial class LoginPage : IHavePassword
{
    /// <summary>
    /// The secure password for this login page.
    /// </summary>
    public SecureString SecurePassword => PasswordText.SecurePassword;

    /// <summary>
    /// Initializes a new instance of the <see cref="LoginPage"/> class.
    /// </summary>
    public LoginPage()
    {
        InitializeComponent();
    }
}