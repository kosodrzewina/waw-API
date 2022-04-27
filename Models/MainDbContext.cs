using Microsoft.EntityFrameworkCore;
using WawAPI.Configurations;

namespace WawAPI.Models;

public class MainDbContext : DbContext
{
    public MainDbContext()
    {
    }

    public MainDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Event> Events { get; set; }
    public DbSet<EventType> EventTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlServer("Server=(LocalDB)\\WawServer; Database=WawDB");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new EventTypeConfiguration());
        modelBuilder.ApplyConfiguration(new EventTypeTypeConfiguration());
    }
}