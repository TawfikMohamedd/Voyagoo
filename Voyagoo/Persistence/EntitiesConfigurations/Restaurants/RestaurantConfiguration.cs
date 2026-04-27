using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Voyagoo.Entities.Restaurants;

namespace Voyagoo.Persistence.EntitiesConfigurations.Restaurants
{
    public class RestaurantConfiguration : IEntityTypeConfiguration<Restaurant>
    {
        public void Configure(EntityTypeBuilder<Restaurant> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(2000).IsRequired();
            builder.Property(x => x.Address).HasMaxLength(500).IsRequired();

            builder.HasMany(x => x.Images)
                   .WithOne(x => x.Restaurant)
                   .HasForeignKey(x => x.RestaurantId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Comments)
                   .WithOne(x => x.Restaurant)
                   .HasForeignKey(x => x.RestaurantId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
