using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Voyagoo.Entities.Restaurants;

namespace Voyagoo.Persistence.EntitiesConfigurations.Restaurants
{
    public class RestaurantCommentConfiguration : IEntityTypeConfiguration<RestaurantComment>
    {
        public void Configure(EntityTypeBuilder<RestaurantComment> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Content).HasMaxLength(1000).IsRequired();

            builder.HasOne(x => x.User)
                   .WithMany()
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
