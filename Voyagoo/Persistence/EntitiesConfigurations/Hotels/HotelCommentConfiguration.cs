using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Voyagoo.Entities.Hotels;

namespace Voyagoo.Persistence.EntitiesConfigurations.Hotels
{
    public class HotelCommentConfiguration : IEntityTypeConfiguration<HotelComment>
    {
        public void Configure(EntityTypeBuilder<HotelComment> builder)
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