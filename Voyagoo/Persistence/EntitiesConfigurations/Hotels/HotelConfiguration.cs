using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Voyagoo.Entities.Hotels;

namespace Voyagoo.Persistence.EntitiesConfigurations.Hotels
{
    public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(2000).IsRequired();
            builder.Property(x => x.Location).HasMaxLength(300).IsRequired();

            builder.HasMany(x => x.Images)
                   .WithOne(x => x.Hotel)
                   .HasForeignKey(x => x.HotelId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Comments)
                   .WithOne(x => x.Hotel)
                   .HasForeignKey(x => x.HotelId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.SinglePrice).HasColumnType("decimal(10,2)");
            builder.Property(x => x.DoublePrice).HasColumnType("decimal(10,2)");
            builder.Property(x => x.TriplePrice).HasColumnType("decimal(10,2)");
            builder.Property(x => x.SuitePrice).HasColumnType("decimal(10,2)");
            builder.Property(x => x.Discount).HasColumnType("decimal(5,2)");
            builder.Property(x => x.ServiceCharge).HasColumnType("decimal(5,2)");
        }
    }
}
