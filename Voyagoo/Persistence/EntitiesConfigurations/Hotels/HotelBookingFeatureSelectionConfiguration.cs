using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Voyagoo.Entities.Hotels;

namespace Voyagoo.Persistence.EntitiesConfigurations.Hotels
{
    public class HotelBookingFeatureSelectionConfiguration : IEntityTypeConfiguration<HotelBookingFeatureSelection>
    {
        public void Configure(EntityTypeBuilder<HotelBookingFeatureSelection> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.PricePerNight).HasColumnType("decimal(10,2)");

            builder.HasOne(x => x.BookingFeature)
                   .WithMany()
                   .HasForeignKey(x => x.BookingFeatureId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}