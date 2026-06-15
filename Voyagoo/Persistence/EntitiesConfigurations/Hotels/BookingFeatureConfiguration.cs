using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Voyagoo.Abstractions.Consts;
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

            builder.HasData(
                new BookingFeature
                {
                    Id = DefaultBookingFeatures.FullBoardId,
                    Name = DefaultBookingFeatures.FullBoardName,
                    Icon = DefaultBookingFeatures.FullBoardIcon
                },
                new BookingFeature
                {
                    Id = DefaultBookingFeatures.HalfBoardId,
                    Name = DefaultBookingFeatures.HalfBoardName,
                    Icon = DefaultBookingFeatures.HalfBoardIcon
                }
            );
        }
    }
}