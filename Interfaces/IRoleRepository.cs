using Website_Progress.Areas.Admin.Models;

namespace Website_Progress.Interfaces
{
    public interface IRoleRepository
    {
        List<Role> GetAll();

        Role? TryGetByName(string roleName);

        Role? TryGetById(Guid roleId);

        void Add(Role role);

        void Delete(Guid roleId);
    }
}
