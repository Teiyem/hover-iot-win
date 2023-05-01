using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Hover.Helpers;

/// <summary>
/// Helpers for the <see cref="SecureString"/> class.
/// </summary>
public static class SecureStringHelpers
{
    /// <summary>
    /// Unsecures a <see cref="SecureString"/> to plain text.
    /// </summary>
    /// <param name="secureString">The secure string to convert to plain text.</param>
    /// <returns></returns>
    public static string Unsecure(this SecureString secureString)
    {
        if (secureString == null)
            return string.Empty;

        var unmanagedString = IntPtr.Zero;

        try
        {
            unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureString);
            return Marshal.PtrToStringUni(unmanagedString);
        }
        finally
        {
            Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
        }
    }
}