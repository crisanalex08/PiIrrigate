using PiIrrigateServer.Models;

namespace PiIrrigateServer.Services
{
    public interface IPasswordHasher
    {
        string HashPassword(User user, string password);
        bool VerifyPassword(User user,string hashedPassword, string providedPassword);
    }

    public class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(User user, string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var passwordHash = Convert.ToBase64String(sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)));
                return passwordHash;
            }
        }

        public bool VerifyPassword(User user, string hashedPassword, string providedPassword)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var providedPasswordHash = Convert.ToBase64String(sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(providedPassword)));
                return hashedPassword == providedPasswordHash;
            }
        }
    }
}
