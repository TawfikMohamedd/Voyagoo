using Voyagoo.Entities.Attractions;

namespace Voyagoo.Entities.BudgetPlanning
{
    public class BudgetPlanAttraction
    {
        public int Id { get; set; }

        public int BudgetPlanId { get; set; }
        public BudgetPlan BudgetPlan { get; set; } = default!;

        public int? AttractionId { get; set; }
        public Attraction? Attraction { get; set; }

        // Snapshot data in case the attraction gets deleted later
        public string AttractionNameSnapshot { get; set; } = string.Empty;
        public decimal TicketPriceSnapshot { get; set; }
    }
}
