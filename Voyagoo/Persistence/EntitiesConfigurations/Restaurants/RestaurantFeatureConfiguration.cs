using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Voyagoo.Entities.Restaurants;

namespace Voyagoo.Persistence.EntitiesConfigurations.Restaurants
{
    public class RestaurantFeatureConfiguration : IEntityTypeConfiguration<RestaurantFeature>
    {
        public void Configure(EntityTypeBuilder<RestaurantFeature> builder)
        {
            // Composite Primary Key
            builder.HasKey(x => new { x.RestaurantId, x.FeatureId });

            builder.HasOne(x => x.Restaurant)
                   .WithMany(x => x.Features)
                   .HasForeignKey(x => x.RestaurantId);

            builder.HasOne(x => x.Feature)
                   .WithMany(x => x.RestaurantFeatures)
                   .HasForeignKey(x => x.FeatureId);
        }
    }
}
