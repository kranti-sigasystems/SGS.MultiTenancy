namespace SGS.MultiTenancy.Core.Services.ServiceInterface
{
    /// <summary>
    /// Defines methods for securely hashing passwords and verifying password hashes.
    /// </summary>
    /// <remarks>The interface is intended to abstract password hashing logic for authentication
    /// systems.</remarks>
    public interface IPasswordHasherService
    {
        /// <summary>
        /// Generates a secure hash for the specified password.
        /// </summary>
        /// <param name="password">The plain text password to hash.</param>
        /// <returns>A string containing the hashed representation of the password.</returns>
        string HashPassword(string password);
        /// <summary>
        /// Verifies whether the specified password matches the provided hashed password.
        /// </summary>
        /// <param name="password">The plain text password to verify.</param>
        /// <param name="hashedPassword">The hashed password to compare against. Cannot be null.</param>
        /// <returns>true if the password matches the hashed password; otherwise, false.</returns>
        bool VerifyPassword(string password, string hashedPassword);
    }
}
