using Website_Progress.Areas.Admin.Models;
using Website_Progress.Interfaces;
using Website_Progress.Models;

namespace Website_Progress.Repositories
{
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly List<User> _users = [];


        public void Add(User user)
        {
            user.Id = Guid.NewGuid();
            user.CreationDateTime = DateTime.Now;

            _users.Add(user);
        }


        public User? TryGetByLogin(string login) =>
            _users.FirstOrDefault(user => user.Login == login);


        public List<User> GetAll() => _users;


        public User? TryGetById(Guid userId) =>
            _users.FirstOrDefault(user => user.Id == userId);


        public void Delete(Guid userId)
        {
            var existingUser = TryGetById(userId);

            if (existingUser != null)
            {
                _users.Remove(existingUser);
            }
        }

        public void Update(User user)
        {
            var existingUser = TryGetById(user.Id);

            if (existingUser != null)
            {
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                existingUser.Phone = user.Phone;
            }
        }


        public void ChangePassword(string login, string newPassword)
        {
            var existingUser = TryGetByLogin(login);

            if (existingUser != null)
            {
                existingUser.Password = newPassword;
            }
        }

        public void ChangeRole(string login, Role? newRole)
        {
            var existingUser = TryGetByLogin(login);

            if (existingUser != null)
            {
                existingUser.Role = newRole;
            }
        }
    }
}

