using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Voyagoo.Entities.Hotels;

namespace Voyagoo.Persistence.EntitiesConfigurations.Hotels
{
    public class HotelBookingRoomConfiguration : IEntityTypeConfiguration<HotelBookingRoom>
    {
        public void Configure(EntityTypeBuilder<HotelBookingRoom> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.PricePerNight).HasColumnType("decimal(10,2)");
        }
    }
}