using System.Security;

namespace Hover.Models;

#region Attribution
// Code attribution: Original Author Luke Malpass (Angel Six)
#endregion

/// <summary>
/// An interface for a class that can provide a secure password
/// </summary>
public interface IHavePassword
{
    /// <summary>
    /// The secure password
    /// </summary>
    SecureString SecurePassword { get; }
}