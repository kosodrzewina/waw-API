using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WawAPI.Models;

namespace WawAPI.Configurations;

public class EventTypeTypeConfiguration : IEntityTypeConfiguration<EventType>
{
    public void Configure(EntityTypeBuilder<EventType> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedOnAdd();
        builder.Property(e => e.Name).IsRequired().HasMaxLength(128);

        builder.HasMany(et => et.Events).WithMany(e => e.Types);

        builder.HasData(
            Enumeration
                .GetAll<EventTypeEnum>()
                .Select(e => new EventType { Id = e.Id, Name = e.Name })
        );
    }
}
