namespace Defra.Imports.Specs.Config
{
    using System;

    /// <summary>
    /// Configuration for a user.
    /// </summary>
    public class CredentialConfiguration
    {
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Validates the credentials.
        /// </summary>
        /// <exception cref="Exception">Thrown if the username or password are null or empty.</exception>
        public void Validate()
        {
            if (string.IsNullOrEmpty(this.Username))
            {
                throw new System.Exception("A username has not been configured for a credential.");
            }

            if (string.IsNullOrEmpty(this.Password))
            {
                throw new System.Exception($"A password has not been configured for the {this.Username} credential.");
            }
        }
    }
}
