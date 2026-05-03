using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Voyagoo.Entities.TourGuides;

namespace Voyagoo.Persistence.EntitiesConfigurations.TourGuides
{
    public class TourGuideBookingConfiguration : IEntityTypeConfiguration<TourGuideBooking>
    {
        public void Configure(EntityTypeBuilder<TourGuideBooking> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.TotalPrice)
                   .HasColumnType("decimal(10,2)");

            builder.HasOne(x => x.TourGuide)
                   .WithMany(x => x.Bookings)
                   .HasForeignKey(x => x.TourGuideId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.User)
                   .WithMany()
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
