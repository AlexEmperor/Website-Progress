using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Website_Progress.ModelsDTO;
using Website_Progress.Services;

namespace Website_Progress.DataContext
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            // Database.Migrate();
            Database.EnsureCreated();
        }

        //Доступ к таблицам
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<SiteSetting> SiteSettings { get; set; }

        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<DeliveryUser> DeliveryUsers { get; set; } = null!;
        public DbSet<News> News { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings =>
                warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }

    }
}
