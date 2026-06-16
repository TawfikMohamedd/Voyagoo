using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Voyagoo.Entities.Hotels;

namespace Voyagoo.Persistence.EntitiesConfigurations.Hotels
{
    public class HotelBookingConfiguration : IEntityTypeConfiguration<HotelBooking>
    {
        public void Configure(EntityTypeBuilder<HotelBooking> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.RoomsTotal).HasColumnType("decimal(10,2)");
            builder.Property(x => x.BoardsTotal).HasColumnType("decimal(10,2)");
            builder.Property(x => x.ExtrasTotal).HasColumnType("decimal(10,2)");
            builder.Property(x => x.Subtotal).HasColumnType("decimal(10,2)");
            builder.Property(x => x.DiscountPercentage).HasColumnType("decimal(5,2)");
            builder.Property(x => x.DiscountAmount).HasColumnType("decimal(10,2)");
            builder.Property(x => x.ServiceChargePercentage).HasColumnType("decimal(5,2)");
            builder.Property(x => x.ServiceChargeAmount).HasColumnType("decimal(10,2)");
            builder.Property(x => x.TotalPrice).HasColumnType("decimal(10,2)");

            builder.HasOne(x => x.Hotel)
                   .WithMany()
                   .HasForeignKey(x => x.HotelId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.User)
                   .WithMany()
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Rooms)
                   .WithOne(x => x.HotelBooking)
                   .HasForeignKey(x => x.HotelBookingId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.SelectedFeatures)
                   .WithOne(x => x.HotelBooking)
                   .HasForeignKey(x => x.HotelBookingId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}