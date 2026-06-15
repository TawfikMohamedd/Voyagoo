using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Voyagoo.Entities.Hotels;

namespace Voyagoo.Persistence.EntitiesConfigurations.Hotels
{
    public class BookingFeatureConfiguration : IEntityTypeConfiguration<BookingFeature>
    {
        public void Configure(EntityTypeBuilder<BookingFeature> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Icon).HasMaxLength(50);
        }
    }
}