using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Voyagoo.Entities.BudgetPlanning;

namespace Voyagoo.Persistence.EntitiesConfigurations.BudgetPlanning
{
    public class BudgetPlanAttractionConfiguration : IEntityTypeConfiguration<BudgetPlanAttraction>
    {
        public void Configure(EntityTypeBuilder<BudgetPlanAttraction> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.AttractionNameSnapshot).HasMaxLength(200);
            builder.Property(x => x.TicketPriceSnapshot).HasColumnType("decimal(10,2)");

            // Attraction can be deleted later, so we use SetNull instead of cascading the delete
            builder.HasOne(x => x.Attraction)
                   .WithMany()
                   .HasForeignKey(x => x.AttractionId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}