using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<Spacecraft> Spacecrafts { get; set; }
    public DbSet<Component> Components { get; set; }
    public DbSet<ComponentDependency> ComponentDependencies { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source=localhost\\SpaceSurfer;Initial Catalog=SpaceXPM;User Id=sa;Password=kalynn;TrustServerCertificate=True;"); // For SQL Server
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure self-referencing many-to-many for Component dependencies
        modelBuilder.Entity<ComponentDependency>()
            .HasKey(cd => new { cd.ComponentId, cd.DependencyId });

        modelBuilder.Entity<ComponentDependency>()
            .HasOne(cd => cd.Component)
            .WithMany(c => c.ComponentDependencies)
            .HasForeignKey(cd => cd.ComponentId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ComponentDependency>()
            .HasOne(cd => cd.Dependency)
            .WithMany(c => c.DependentOn)
            .HasForeignKey(cd => cd.DependencyId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
