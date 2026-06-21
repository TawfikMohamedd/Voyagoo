using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Voyagoo.Entities.BudgetPlanning;

namespace Voyagoo.Persistence.EntitiesConfigurations.BudgetPlanning
{
    public class BudgetPlanRestaurantConfiguration : IEntityTypeConfiguration<BudgetPlanRestaurant>
    {
        public void Configure(EntityTypeBuilder<BudgetPlanRestaurant> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.RestaurantNameSnapshot).HasMaxLength(200);
            builder.Property(x => x.EstimatedPriceSnapshot).HasColumnType("decimal(10,2)");

            // Restaurant can be deleted later, so we use SetNull instead of cascading the delete
            builder.HasOne(x => x.Restaurant)
                   .WithMany()
                   .HasForeignKey(x => x.RestaurantId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
