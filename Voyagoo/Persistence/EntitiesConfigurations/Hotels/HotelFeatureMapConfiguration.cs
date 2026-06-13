using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Voyagoo.Entities.Hotels;

namespace Voyagoo.Persistence.EntitiesConfigurations.Hotels
{
    public class HotelFeatureMapConfiguration : IEntityTypeConfiguration<HotelFeatureMap>
    {
        public void Configure(EntityTypeBuilder<HotelFeatureMap> builder)
        {
            // Composite Primary Key
            builder.HasKey(x => new { x.HotelId, x.HotelFeatureId });

            builder.HasOne(x => x.Hotel)
                   .WithMany(x => x.Features)
                   .HasForeignKey(x => x.HotelId);

            builder.HasOne(x => x.HotelFeature)
                   .WithMany(x => x.HotelFeatures)
                   .HasForeignKey(x => x.HotelFeatureId);
        }
    }
}
