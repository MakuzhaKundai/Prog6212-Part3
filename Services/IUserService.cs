using Contract_Monthly_Claim_System.Models;

namespace Contract_Monthly_Claim_System.Services
{
    public interface IUserService
    {
        IEnumerable<User> GetAllUsers();
        User? GetUserById(int id);
        void AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(int id);
        User? Authenticate(string username, string password);
        bool UserExists(int id);
    }
}