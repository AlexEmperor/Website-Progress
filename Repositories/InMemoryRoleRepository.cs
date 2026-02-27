using Website_Progress.Areas.Admin.Models;
using Website_Progress.Interfaces;

namespace Website_Progress.Repositories
{
   /* public class InMemoryRoleRepository : IRoleRepository
    {
        private readonly List<Role> _roles = [];

        public InMemoryRoleRepository()
        {
            _roles =
            [
                new Role(){Id = Guid.NewGuid(), Name = "Admin"},
                new Role(){Id = Guid.NewGuid(), Name = "Moderator"},
                new Role(){Id = Guid.NewGuid(), Name = "User"},
                new Role(){Id = Guid.NewGuid(), Name = "Developer"},
                new Role(){Id = Guid.NewGuid(), Name = "Guest"},
            ];
        }

        public void Add(Role role)
        {
            role.Id = Guid.NewGuid();

            _roles.Add(role);
        }

        public void Delete(Guid roleId)
        {
            var existingRole = TryGetById(roleId);

            if (existingRole != null)
            {
                _roles.Remove(existingRole);
            }
        }

        public List<Role> GetAll() => _roles;


        public Role? TryGetById(Guid roleId) =>
            _roles.FirstOrDefault(role => role.Id == roleId);

        public Role? TryGetByName(string roleName) =>
            _roles.FirstOrDefault(role => role.Name == roleName);
    }*/
}
