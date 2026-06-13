using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Voyagoo.Entities.Favorites;

namespace Voyagoo.Persistence.EntitiesConfigurations.Favorites
{
    public class FavoriteConfiguration : IEntityTypeConfiguration<Favorite>
    {
        public void Configure(EntityTypeBuilder<Favorite> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.User)
                   .WithMany()
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Restaurant)
                   .WithMany()
                   .HasForeignKey(x => x.RestaurantId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.TourGuide)
                   .WithMany()
                   .HasForeignKey(x => x.TourGuideId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Attraction)
                   .WithMany()
                   .HasForeignKey(x => x.AttractionId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Hotel)
                   .WithMany()
                   .HasForeignKey(x => x.HotelId)
                   .OnDelete(DeleteBehavior.NoAction);
            // يوزر ميقدرش يعمل favorite نفس المطعم مرتين
            builder.HasIndex(x => new { x.UserId, x.RestaurantId })
                   .IsUnique()
                   .HasFilter("[RestaurantId] IS NOT NULL");

            builder.HasIndex(x => new { x.UserId, x.TourGuideId })
                   .IsUnique()
                   .HasFilter("[TourGuideId] IS NOT NULL");

            builder.HasIndex(x => new { x.UserId, x.AttractionId })
                   .IsUnique()
                   .HasFilter("[AttractionId] IS NOT NULL");

            builder.HasIndex(x => new { x.UserId, x.HotelId })
                   .IsUnique()
                   .HasFilter("[HotelId] IS NOT NULL");
        }
    }
}
