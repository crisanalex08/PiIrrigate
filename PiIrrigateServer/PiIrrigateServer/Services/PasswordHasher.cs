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
            return password;
        }

        public bool VerifyPassword(User user, string hashedPassword, string providedPassword)
        {
            throw new NotImplementedException();
        }
    }
}
