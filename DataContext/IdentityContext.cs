namespace Website_Progress.DataContext
{
    public class IdentityContext(DbContextOptions<IdentityContext> options) : IdentityDbContext<UserDTO>(options)
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.ConfigureWarnings(warnings =>
            warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }
    }
}
