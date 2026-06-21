using Voyagoo.Entities.Restaurants;

namespace Voyagoo.Entities.BudgetPlanning
{
    public class BudgetPlanRestaurant
    {
        public int Id { get; set; }

        public int BudgetPlanId { get; set; }
        public BudgetPlan BudgetPlan { get; set; } = default!;

        public int? RestaurantId { get; set; }
        public Restaurant? Restaurant { get; set; }

        // Snapshot data in case the restaurant gets deleted later
        public string RestaurantNameSnapshot { get; set; } = string.Empty;
        public decimal EstimatedPriceSnapshot { get; set; }
    }
}
