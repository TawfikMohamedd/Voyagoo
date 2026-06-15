using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Voyagoo.Entities.Hotels;

namespace Voyagoo.Persistence.EntitiesConfigurations.Hotels
{
    public class HotelBookingFeatureConfiguration : IEntityTypeConfiguration<HotelBookingFeature>
    {
        public void Configure(EntityTypeBuilder<HotelBookingFeature> builder)
        {
            // Composite Primary Key
            builder.HasKey(x => new { x.HotelId, x.BookingFeatureId });

            builder.Property(x => x.Price).HasColumnType("decimal(10,2)");

            builder.HasOne(x => x.Hotel)
                   .WithMany(x => x.BookingFeatures)
                   .HasForeignKey(x => x.HotelId);

            builder.HasOne(x => x.BookingFeature)
                   .WithMany(x => x.HotelBookingFeatures)
                   .HasForeignKey(x => x.BookingFeatureId);
        }
    }
}
