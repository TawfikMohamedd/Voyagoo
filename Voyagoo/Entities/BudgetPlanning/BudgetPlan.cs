using Voyagoo.Entities.Hotels;

namespace Voyagoo.Entities.BudgetPlanning
{
    public class BudgetPlan
    {
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = default!;

        public decimal TotalBudget { get; set; }
        public int NumberOfDays { get; set; }

        public decimal HotelBudget { get; set; }
        public decimal RestaurantBudget { get; set; }
        public decimal AttractionBudget { get; set; }

        // Selected Hotel (snapshot, nullable in case the hotel gets deleted)
        public int? HotelId { get; set; }
        public Hotel? Hotel { get; set; }
        public string HotelNameSnapshot { get; set; } = string.Empty;
        public decimal HotelPriceSnapshot { get; set; }

        public List<BudgetPlanRestaurant> Restaurants { get; set; } = [];
        public List<BudgetPlanAttraction> Attractions { get; set; } = [];

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}