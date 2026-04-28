using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Voyagoo.Entities.Restaurants;

namespace Voyagoo.Persistence.EntitiesConfigurations.Restaurants
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.GuestName).HasMaxLength(100).IsRequired();
            builder.Property(x => x.GuestPhone).HasMaxLength(11).IsRequired();

            builder.HasOne(x => x.Restaurant)
                   .WithMany()
                   .HasForeignKey(x => x.RestaurantId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.User)
                   .WithMany()
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
