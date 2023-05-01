using System.Security;
using Hover.Models;

namespace Hover.Views;

/// <summary>
/// The register page.
/// </summary>
public partial class RegisterPage : IHavePassword
{
    /// <summary>
    /// The secure password for this register page.
    /// </summary>
    public SecureString SecurePassword => PasswordText.SecurePassword;

    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterPage"/> class.
    /// </summary>
    public RegisterPage()
    {
        InitializeComponent();
    }
}