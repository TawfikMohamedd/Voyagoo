using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Voyagoo.Entities.BudgetPlanning;

namespace Voyagoo.Persistence.EntitiesConfigurations.BudgetPlanning
{
    public class BudgetPlanConfiguration : IEntityTypeConfiguration<BudgetPlan>
    {
        public void Configure(EntityTypeBuilder<BudgetPlan> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.TotalBudget).HasColumnType("decimal(10,2)");
            builder.Property(x => x.HotelBudget).HasColumnType("decimal(10,2)");
            builder.Property(x => x.RestaurantBudget).HasColumnType("decimal(10,2)");
            builder.Property(x => x.AttractionBudget).HasColumnType("decimal(10,2)");
            builder.Property(x => x.HotelPriceSnapshot).HasColumnType("decimal(10,2)");
            builder.Property(x => x.HotelNameSnapshot).HasMaxLength(200);

            builder.HasOne(x => x.User)
                   .WithMany()
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Hotel can be deleted later, so we use SetNull instead of cascading the delete
            builder.HasOne(x => x.Hotel)
                   .WithMany()
                   .HasForeignKey(x => x.HotelId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(x => x.Restaurants)
                   .WithOne(x => x.BudgetPlan)
                   .HasForeignKey(x => x.BudgetPlanId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Attractions)
                   .WithOne(x => x.BudgetPlan)
                   .HasForeignKey(x => x.BudgetPlanId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
