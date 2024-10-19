using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<Spacecraft> Spacecrafts { get; set; }
    public DbSet<Component> Components { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source=localhost\\SpaceSurfer;Initial Catalog=SpaceXPM;User Id=sa;Password=kalynn;TrustServerCertificate=True;"); // For SQL Server
    }
}
