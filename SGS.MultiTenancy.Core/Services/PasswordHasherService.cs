using SGS.MultiTenancy.Core.Services.ServiceInterface;
using System.Security.Cryptography;
using System.Text;

namespace SGS.MultiTenancy.Core.Services
{
    public class PasswordHasherService : IPasswordHasherService
    {
        /// <summary>
        /// Hashes a password.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            byte[] inputBytes = Encoding.UTF8.GetBytes(password);
            byte[] hashBytes = sha256.ComputeHash(inputBytes);
            return Convert.ToHexString(hashBytes);
        }

        /// <summary>
        /// Verigy the given hashed password with the stored hash.
        /// </summary>
        /// <param name="enteredPassword"></param>
        /// <param name="storedHash"></param>
        /// <returns></returns>
        public bool VerifyPassword(string enteredPassword, string storedHash)
        {
            string enteredPasswordHash = HashPassword(enteredPassword);
            return enteredPasswordHash.Equals(storedHash, StringComparison.OrdinalIgnoreCase);
        }
    }
}
