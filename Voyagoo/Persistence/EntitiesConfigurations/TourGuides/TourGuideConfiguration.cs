using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Voyagoo.Entities.TourGuides;

namespace Voyagoo.Persistence.EntitiesConfigurations.TourGuides
{
    public class TourGuideConfiguration : IEntityTypeConfiguration<TourGuide>
    {
        public void Configure(EntityTypeBuilder<TourGuide> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                   .HasMaxLength(200)
                   .IsRequired();

            builder.Property(x => x.Email)
                   .HasMaxLength(256)
                   .IsRequired();

            builder.Property(x => x.PhoneNumber)
                   .HasMaxLength(11)
                   .IsRequired();

            builder.Property(x => x.Description)
                   .HasMaxLength(2000)
                   .IsRequired();

            builder.Property(x => x.Rating)
                   .IsRequired();

            builder.Property(x => x.ProfilePictureUrl)
                   .HasMaxLength(500);

            // نخزن الـ Languages كـ comma-separated string في column واحد
            builder.Property(x => x.Languages)
       .HasConversion(
           v => string.Join(',', v.Select(l => (int)l)),
           v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                 .Select(x => (Language)int.Parse(x))
                 .ToList()
       )
       .HasMaxLength(200)
       .Metadata.SetValueComparer(new ValueComparer<List<Language>>(
           (c1, c2) => c1!.SequenceEqual(c2!),
           c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
           c => c.ToList()
       ));

            builder.Property(x => x.PricePerDay).HasColumnType("decimal(18,2)");
        }
    }
}
