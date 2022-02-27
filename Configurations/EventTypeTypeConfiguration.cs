using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WawAPI.Configurations;

public class EventTypeTypeConfiguration : IEntityTypeConfiguration<Models.EventType>
{
    public void Configure(EntityTypeBuilder<Models.EventType> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedOnAdd();
        builder.Property(e => e.Name).IsRequired().HasMaxLength(128);

        builder.HasMany(et => et.Events).WithOne(e => e.Type).HasForeignKey(e => e.IdEventType);

        builder.HasData(
            Enumeration.GetAll<EventTypeEnum>().Select(e =>
               new Models.EventType
               {
                   Id = e.Id,
                   Name = e.Name
               }
            )
        );
    }
}
