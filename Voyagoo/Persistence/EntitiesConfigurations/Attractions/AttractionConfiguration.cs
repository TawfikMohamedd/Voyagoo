using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Voyagoo.Entities.Attractions;

namespace Voyagoo.Persistence.EntitiesConfigurations.Attractions
{
    public class AttractionConfiguration : IEntityTypeConfiguration<Attraction>
    {
        public void Configure(EntityTypeBuilder<Attraction> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(2000).IsRequired();
            builder.Property(x => x.Place).HasMaxLength(300).IsRequired();
            builder.Property(x => x.TicketPrice).HasColumnType("decimal(10,2)");

            builder.HasMany(x => x.Images)
                   .WithOne(x => x.Attraction)
                   .HasForeignKey(x => x.AttractionId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
