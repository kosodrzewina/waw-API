using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WawAPI.Models;

namespace WawAPI.Configurations;

public class LocationTypeConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.HasKey(l => l.IdEvent);
        builder.Property(l => l.Latitude).IsRequired();
        builder.Property(l => l.Longitude).IsRequired();

        builder.HasOne(l => l.Event).WithOne(e => e.Location);
    }
}
