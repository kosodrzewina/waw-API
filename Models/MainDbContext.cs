using Microsoft.EntityFrameworkCore;
using WawAPI.Configurations;

namespace WawAPI.Models;

public class MainDbContext : DbContext
{
    public MainDbContext() { }

    public MainDbContext(DbContextOptions options) : base(options) { }

    public DbSet<Event> Events { get; set; } = default!;
    public DbSet<EventType> EventTypes { get; set; } = default!;
    public DbSet<Location> Locations { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new EventTypeConfiguration());
        modelBuilder.ApplyConfiguration(new EventTypeTypeConfiguration());
        modelBuilder.ApplyConfiguration(new LocationTypeConfiguration());
    }
}
