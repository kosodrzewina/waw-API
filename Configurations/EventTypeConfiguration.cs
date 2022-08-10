using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WawAPI.Models;

namespace WawAPI.Configurations;

public class EventTypeConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedOnAdd();
        builder.Property(e => e.Title).IsRequired().HasMaxLength(250);
        builder.Property(e => e.Description).IsRequired().HasMaxLength(8000);
        builder.Property(e => e.Link).IsRequired().HasMaxLength(1000);
        builder.Property(e => e.Address).IsRequired().HasMaxLength(1000);
        builder.Property(e => e.Image).IsRequired();
        builder.Property(e => e.Guid).IsRequired().HasMaxLength(1000);
        builder.Property(e => e.IsCurrent).IsRequired();

        builder.HasMany(e => e.Types).WithMany(t => t.Events);
        builder
            .HasOne(e => e.Location)
            .WithOne(l => l.Event)
            .HasForeignKey<Location>(l => l.IdEvent);
    }
}
