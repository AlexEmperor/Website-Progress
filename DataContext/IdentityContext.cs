using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Website_Progress.ModelsDTO;

namespace Website_Progress.DataContext
{
    public class IdentityContext : IdentityDbContext<UserDTO>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        {
            Database.Migrate();
        }
    }
}
