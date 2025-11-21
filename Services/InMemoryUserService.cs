using Contract_Monthly_Claim_System.Models;

namespace Contract_Monthly_Claim_System.Services
{
    public class InMemoryUserService : IUserService
    {
        private readonly List<User> _users;

        public InMemoryUserService()
        {
            _users = new List<User>
            {
                new User
                {
                    UserId = 1,
                    Username = "admin",
                    Password = "admin123",
                    Role = "Admin",
                    Email = "admin@university.com",
                    FirstName = "System",
                    LastName = "Administrator",
                    IsActive = true,
                    DateCreated = DateTime.Now
                },
                new User
                {
                    UserId = 2,
                    Username = "lecturer1",
                    Password = "lecturer123",
                    Role = "Lecturer",
                    Email = "lecturer1@university.com",
                    FirstName = "John",
                    LastName = "Smith",
                    HourlyRate = 50,
                    IsActive = true,
                    DateCreated = DateTime.Now
                },
                new User
                {
                    UserId = 3,
                    Username = "lecturer2",
                    Password = "lecturer123",
                    Role = "Lecturer",
                    Email = "lecturer2@university.com",
                    FirstName = "Sarah",
                    LastName = "Johnson",
                    HourlyRate = 45,
                    IsActive = true,
                    DateCreated = DateTime.Now
                }
            };
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _users.Where(u => u.IsActive);
        }

        public User? GetUserById(int id)
        {
            return _users.FirstOrDefault(u => u.UserId == id && u.IsActive);
        }

        public void AddUser(User user)
        {
            user.UserId = _users.Count + 1;
            user.DateCreated = DateTime.Now;
            _users.Add(user);
        }

        public void UpdateUser(User user)
        {
            var existingUser = GetUserById(user.UserId);
            if (existingUser != null)
            {
                existingUser.Username = user.Username;
                existingUser.Email = user.Email;
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                existingUser.Role = user.Role;
                existingUser.HourlyRate = user.HourlyRate;
                existingUser.IsActive = user.IsActive;
            }
        }

        public void DeleteUser(int id)
        {
            var user = GetUserById(id);
            if (user != null)
            {
                user.IsActive = false;
            }
        }

        public User? Authenticate(string username, string password)
        {
            return _users.FirstOrDefault(u => u.Username == username && u.Password == password && u.IsActive);
        }

        public bool UserExists(int id)
        {
            return _users.Any(u => u.UserId == id && u.IsActive);
        }
    }
}