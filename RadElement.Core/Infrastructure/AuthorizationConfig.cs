using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;

namespace RadElement.Core.Infrastructure
{
    /// <summary>
    /// Constains the details for configuring authorization
    /// </summary>
    public class AuthorizationConfig
    {
        /// <summary>
        /// Gets or sets the domain
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Gets or sets the audience
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// Gets or sets the key file
        /// </summary>
        public string KeyFilePath { get; set; }

        /// <summary>
        /// Gets or sets the Key
        /// </summary>
        public SecureString SigningPassword { get; private set; }

        /// <summary>
        /// Sets the key file password.
        /// </summary>
        /// <param name="input">The input.</param>
        public void SetKeyFilePassword(string input)
        {
            SigningPassword = new SecureString();
            input.ToCharArray().ToList().ForEach((q) => SigningPassword.AppendChar(q));
        }

        /// <summary>
        /// Converts to unsecure string.
        /// </summary>
        /// <param name="securePassword">The secure password.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">securePassword</exception>
        public string ConvertToUnsecureString(SecureString securePassword)
        {
            if (securePassword == null)
                throw new ArgumentNullException("securePassword");

            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
    }
}
