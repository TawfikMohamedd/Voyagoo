using Microsoft.EntityFrameworkCore;
using Voyagoo.Abstractions;
using Voyagoo.Abstractions.Consts;
using Voyagoo.Contracts.BudgetPlanning;
using Voyagoo.Entities.Attractions;
using Voyagoo.Entities.BudgetPlanning;
using Voyagoo.Entities.Hotels;
using Voyagoo.Entities.Restaurants;
using Voyagoo.Errors;
using Voyagoo.Persistence;

namespace Voyagoo.Services
{
    public class BudgetPlanService(VoyagooDbContext context) : IBudgetPlanService
    {
        private readonly VoyagooDbContext _context = context;

        public async Task<Result<GetMinimumBudgetResponse>> GetMinimumBudgetAsync(
            GetMinimumBudgetRequest request,
            CancellationToken cancellationToken = default)
        {
            if (request.NumberOfDays <= 0)
                return Result.Failure<GetMinimumBudgetResponse>(BudgetPlanErrors.InvalidNumberOfDays);

            var minimums = await CalculateMinimumBudgetBreakdownAsync(request.NumberOfDays, cancellationToken);

            var response = new GetMinimumBudgetResponse(
                request.NumberOfDays,
                minimums.MinimumTotalBudget,
                minimums.MinimumHotelBudget,
                minimums.MinimumRestaurantBudget,
                minimums.MinimumAttractionBudget
            );

            return Result.Success(response);
        }

        public async Task<Result<SuggestBudgetPlanResponse>> SuggestPlanAsync(
            SuggestBudgetPlanRequest request,
            CancellationToken cancellationToken = default)
        {
            var minimums = await CalculateMinimumBudgetBreakdownAsync(request.NumberOfDays, cancellationToken);

            if (request.TotalBudget < minimums.MinimumTotalBudget)
                return Result.Failure<SuggestBudgetPlanResponse>(BudgetPlanErrors.BudgetBelowMinimum);

            var (hotelBudget, restaurantBudget, attractionBudget) = SplitBudget(request.TotalBudget);

            // ── Suggested Hotels ──
            var hotels = await _context.Hotels
                .Where(h => !h.IsDeleted && h.Status == HotelStatus.Active)
                .Include(h => h.Images)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var suggestedHotels = hotels
                .Select(h => new
                {
                    Hotel = h,
                    MinPrice = GetHotelMinPrice(h)
                })
                .Where(x => x.MinPrice > 0 && x.MinPrice * request.NumberOfDays <= hotelBudget)
                .OrderByDescending(x => x.Hotel.Rating)
                .Take(10)
                .Select(x => new SuggestedHotelItem(
                    x.Hotel.Id,
                    x.Hotel.Name,
                    x.Hotel.Location,
                    x.Hotel.Rating,
                    x.MinPrice,
                    x.MinPrice * request.NumberOfDays,
                    x.Hotel.Images.FirstOrDefault(i => i.IsMain)?.ImageUrl
                        ?? x.Hotel.Images.FirstOrDefault()?.ImageUrl
                ))
                .ToList();

            // ── Suggested Restaurants ──
            var restaurants = await _context.Restaurants
                .Where(r => !r.IsDeleted && r.Status == RestaurantStatus.Active)
                .Include(r => r.Images)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var suggestedRestaurants = restaurants
                .Select(r => new
                {
                    Restaurant = r,
                    EstimatedPrice = (r.MinPrice + r.MaxPrice) / 2
                })
                .Where(x => x.EstimatedPrice <= restaurantBudget)
                .OrderByDescending(x => x.Restaurant.Rating)
                .Take(10)
                .Select(x => new SuggestedRestaurantItem(
                    x.Restaurant.Id,
                    x.Restaurant.Name,
                    x.Restaurant.Address,
                    x.Restaurant.Rating,
                    x.Restaurant.CuisineType.ToString(),
                    x.Restaurant.MinPrice,
                    x.Restaurant.MaxPrice,
                    x.EstimatedPrice,
                    x.Restaurant.Images.FirstOrDefault(i => i.IsMain)?.ImageUrl
                        ?? x.Restaurant.Images.FirstOrDefault()?.ImageUrl
                ))
                .ToList();

            // ── Suggested Attractions ──
            var attractions = await _context.Attractions
                .Where(a => !a.IsDeleted && a.Status == AttractionStatus.Active)
                .Include(a => a.Images)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var suggestedAttractions = attractions
                .Where(a => a.TicketPrice <= attractionBudget)
                .OrderByDescending(a => a.Rating)
                .Take(10)
                .Select(a => new SuggestedAttractionItem(
                    a.Id,
                    a.Name,
                    a.Location,
                    a.Rating,
                    a.Category.ToString(),
                    a.TicketPrice,
                    a.Images.FirstOrDefault(i => i.IsMain)?.ImageUrl
                        ?? a.Images.FirstOrDefault()?.ImageUrl
                ))
                .ToList();

            var response = new SuggestBudgetPlanResponse(
                request.TotalBudget,
                request.NumberOfDays,
                hotelBudget,
                restaurantBudget,
                attractionBudget,
                suggestedHotels,
                suggestedRestaurants,
                suggestedAttractions
            );

            return Result.Success(response);
        }

        public async Task<Result<BudgetPlanResponse>> SavePlanAsync(
            string userId,
            SaveBudgetPlanRequest request,
            CancellationToken cancellationToken = default)
        {
            var minimums = await CalculateMinimumBudgetBreakdownAsync(request.NumberOfDays, cancellationToken);

            if (request.TotalBudget < minimums.MinimumTotalBudget)
                return Result.Failure<BudgetPlanResponse>(BudgetPlanErrors.BudgetBelowMinimum);

            var (hotelBudget, restaurantBudget, attractionBudget) = SplitBudget(request.TotalBudget);

            // ── Validate & Snapshot Hotel ──
            var hotel = await _context.Hotels
                .Where(h => h.Id == request.HotelId && !h.IsDeleted && h.Status == HotelStatus.Active)
                .Include(h => h.Images)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            if (hotel is null)
                return Result.Failure<BudgetPlanResponse>(BudgetPlanErrors.InvalidHotelSelection);

            var hotelMinPrice = GetHotelMinPrice(hotel);
            var hotelTotalPrice = hotelMinPrice * request.NumberOfDays;

            if (hotelMinPrice <= 0 || hotelTotalPrice > hotelBudget)
                return Result.Failure<BudgetPlanResponse>(BudgetPlanErrors.HotelExceedsBudget);

            // ── Validate & Snapshot Restaurants ──
            if (request.RestaurantIds.Count == 0)
                return Result.Failure<BudgetPlanResponse>(BudgetPlanErrors.NoRestaurantsSelected);

            var selectedRestaurants = await _context.Restaurants
                .Where(r => request.RestaurantIds.Contains(r.Id) && !r.IsDeleted && r.Status == RestaurantStatus.Active)
                .Include(r => r.Images)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            if (selectedRestaurants.Count != request.RestaurantIds.Distinct().Count())
                return Result.Failure<BudgetPlanResponse>(BudgetPlanErrors.InvalidRestaurantSelection);

            var restaurantsTotal = selectedRestaurants.Sum(r => (r.MinPrice + r.MaxPrice) / 2);

            if (restaurantsTotal > restaurantBudget)
                return Result.Failure<BudgetPlanResponse>(BudgetPlanErrors.RestaurantsExceedBudget);

            // ── Validate & Snapshot Attractions ──
            if (request.AttractionIds.Count == 0)
                return Result.Failure<BudgetPlanResponse>(BudgetPlanErrors.NoAttractionsSelected);

            var selectedAttractions = await _context.Attractions
                .Where(a => request.AttractionIds.Contains(a.Id) && !a.IsDeleted && a.Status == AttractionStatus.Active)
                .Include(a => a.Images)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            if (selectedAttractions.Count != request.AttractionIds.Distinct().Count())
                return Result.Failure<BudgetPlanResponse>(BudgetPlanErrors.InvalidAttractionSelection);

            var attractionsTotal = selectedAttractions.Sum(a => a.TicketPrice);

            if (attractionsTotal > attractionBudget)
                return Result.Failure<BudgetPlanResponse>(BudgetPlanErrors.AttractionsExceedBudget);

            // ── Build & Save the plan ──
            var plan = new BudgetPlan
            {
                UserId = userId,
                TotalBudget = request.TotalBudget,
                NumberOfDays = request.NumberOfDays,
                HotelBudget = hotelBudget,
                RestaurantBudget = restaurantBudget,
                AttractionBudget = attractionBudget,
                HotelId = hotel.Id,
                HotelNameSnapshot = hotel.Name,
                HotelPriceSnapshot = hotelTotalPrice,
                Restaurants = selectedRestaurants.Select(r => new BudgetPlanRestaurant
                {
                    RestaurantId = r.Id,
                    RestaurantNameSnapshot = r.Name,
                    EstimatedPriceSnapshot = (r.MinPrice + r.MaxPrice) / 2
                }).ToList(),
                Attractions = selectedAttractions.Select(a => new BudgetPlanAttraction
                {
                    AttractionId = a.Id,
                    AttractionNameSnapshot = a.Name,
                    TicketPriceSnapshot = a.TicketPrice
                }).ToList()
            };

            await _context.BudgetPlans.AddAsync(plan, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success(MapToResponse(plan, hotel, selectedRestaurants, selectedAttractions));
        }

        public async Task<Result<List<BudgetPlanResponse>>> GetUserPlansAsync(
            string userId,
            CancellationToken cancellationToken = default)
        {
            var plans = await _context.BudgetPlans
                .Where(p => p.UserId == userId)
                .Include(p => p.Hotel).ThenInclude(h => h!.Images)
                .Include(p => p.Restaurants).ThenInclude(r => r.Restaurant).ThenInclude(r => r!.Images)
                .Include(p => p.Attractions).ThenInclude(a => a.Attraction).ThenInclude(a => a!.Images)
                .OrderByDescending(p => p.CreatedAt)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var response = plans.Select(MapPlanWithIncludesToResponse).ToList();

            return Result.Success(response);
        }

        public async Task<Result> DeletePlanAsync(
            string userId,
            int planId,
            CancellationToken cancellationToken = default)
        {
            var plan = await _context.BudgetPlans
                .FirstOrDefaultAsync(p => p.Id == planId, cancellationToken);

            if (plan is null)
                return Result.Failure(BudgetPlanErrors.BudgetPlanNotFound);

            if (plan.UserId != userId)
                return Result.Failure(BudgetPlanErrors.BudgetPlanNotOwned);

            _context.BudgetPlans.Remove(plan);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        // ─────────────────────────────────────────────
        // Helpers
        // ─────────────────────────────────────────────

        private async Task<(decimal MinimumTotalBudget, decimal MinimumHotelBudget, decimal MinimumRestaurantBudget, decimal MinimumAttractionBudget)>
            CalculateMinimumBudgetBreakdownAsync(int numberOfDays, CancellationToken cancellationToken)
        {
            // Pull only the 4 room prices per active hotel — EF Core can translate this projection fine.
            var hotelPrices = await _context.Hotels
                .Where(h => !h.IsDeleted && h.Status == HotelStatus.Active)
                .Select(h => new { h.SinglePrice, h.DoublePrice, h.TriplePrice, h.SuitePrice })
                .ToListAsync(cancellationToken);

            // The "min price per hotel, ignoring zero/unset room types" logic runs client-side
            // (on the already-fetched List<>), since EF Core can't translate array+Where+Min chains to SQL.
            var cheapestHotelPricePerNight = hotelPrices
                .Select(h => new[] { h.SinglePrice, h.DoublePrice, h.TriplePrice, h.SuitePrice }
                    .Where(p => p > 0)
                    .DefaultIfEmpty(0)
                    .Min())
                .Where(p => p > 0)
                .DefaultIfEmpty(0)
                .Min();

            // Cheapest available restaurant (average of its min/max price), charged once regardless of trip length.
            // This is a simple scalar projection, so EF Core can translate it directly.
            var cheapestRestaurantEstimatedPrice = await _context.Restaurants
                .Where(r => !r.IsDeleted && r.Status == RestaurantStatus.Active)
                .Select(r => (r.MinPrice + r.MaxPrice) / 2)
                .OrderBy(p => p)
                .FirstOrDefaultAsync(cancellationToken);

            // Cheapest available attraction ticket, charged once regardless of trip length
            var cheapestAttractionPrice = await _context.Attractions
                .Where(a => !a.IsDeleted && a.Status == AttractionStatus.Active)
                .Select(a => a.TicketPrice)
                .OrderBy(p => p)
                .FirstOrDefaultAsync(cancellationToken);

            var minHotelCost = cheapestHotelPricePerNight * numberOfDays;
            var minRestaurantCost = cheapestRestaurantEstimatedPrice;
            var minAttractionCost = cheapestAttractionPrice;

            // If the database has no data for a category yet, fall back to a flat per-day amount
            // so the feature still works on a fresh/empty database.
            var fallbackPerCategory = BudgetPlanningConsts.FallbackMinimumBudgetPerDay * numberOfDays;

            var minimumHotelBudget = minHotelCost > 0
                ? minHotelCost / BudgetPlanningConsts.HotelPercentage
                : fallbackPerCategory;

            var minimumRestaurantBudget = minRestaurantCost > 0
                ? minRestaurantCost / BudgetPlanningConsts.RestaurantPercentage
                : fallbackPerCategory;

            var minimumAttractionBudget = minAttractionCost > 0
                ? minAttractionCost / BudgetPlanningConsts.AttractionPercentage
                : fallbackPerCategory;

            var minimumTotalBudget = Math.Max(minimumHotelBudget, Math.Max(minimumRestaurantBudget, minimumAttractionBudget));

            return (
                Math.Round(minimumTotalBudget, 2),
                Math.Round(minimumHotelBudget, 2),
                Math.Round(minimumRestaurantBudget, 2),
                Math.Round(minimumAttractionBudget, 2)
            );
        }

        private static (decimal HotelBudget, decimal RestaurantBudget, decimal AttractionBudget) SplitBudget(decimal totalBudget)
        {
            var hotelBudget = totalBudget * BudgetPlanningConsts.HotelPercentage;
            var restaurantBudget = totalBudget * BudgetPlanningConsts.RestaurantPercentage;
            var attractionBudget = totalBudget * BudgetPlanningConsts.AttractionPercentage;

            return (hotelBudget, restaurantBudget, attractionBudget);
        }

        private static decimal GetHotelMinPrice(Hotel hotel)
        {
            var prices = new[] { hotel.SinglePrice, hotel.DoublePrice, hotel.TriplePrice, hotel.SuitePrice }
                .Where(p => p > 0)
                .ToList();

            return prices.Count > 0 ? prices.Min() : 0;
        }

        private static BudgetPlanResponse MapToResponse(
            BudgetPlan plan,
            Hotel hotel,
            List<Restaurant> restaurants,
            List<Attraction> attractions)
        {
            var hotelItem = new BudgetPlanHotelItem(
                hotel.Id,
                hotel.Name,
                plan.HotelPriceSnapshot,
                hotel.Images.FirstOrDefault(i => i.IsMain)?.ImageUrl ?? hotel.Images.FirstOrDefault()?.ImageUrl
            );

            var restaurantItems = restaurants.Select(r => new BudgetPlanRestaurantItem(
                r.Id,
                r.Name,
                (r.MinPrice + r.MaxPrice) / 2,
                r.Images.FirstOrDefault(i => i.IsMain)?.ImageUrl ?? r.Images.FirstOrDefault()?.ImageUrl
            )).ToList();

            var attractionItems = attractions.Select(a => new BudgetPlanAttractionItem(
                a.Id,
                a.Name,
                a.TicketPrice,
                a.Images.FirstOrDefault(i => i.IsMain)?.ImageUrl ?? a.Images.FirstOrDefault()?.ImageUrl
            )).ToList();

            var totalRestaurantCost = restaurantItems.Sum(r => r.EstimatedPrice);
            var totalAttractionCost = attractionItems.Sum(a => a.TicketPrice);

            return new BudgetPlanResponse(
                plan.Id,
                plan.TotalBudget,
                plan.NumberOfDays,
                plan.HotelBudget,
                plan.RestaurantBudget,
                plan.AttractionBudget,
                hotelItem,
                restaurantItems,
                attractionItems,
                plan.HotelPriceSnapshot,
                totalRestaurantCost,
                totalAttractionCost,
                plan.HotelPriceSnapshot + totalRestaurantCost + totalAttractionCost,
                plan.CreatedAt
            );
        }

        private static BudgetPlanResponse MapPlanWithIncludesToResponse(BudgetPlan plan)
        {
            // Falls back to the snapshot data whenever the related entity was deleted (SetNull)
            var hotelItem = new BudgetPlanHotelItem(
                plan.HotelId,
                plan.Hotel?.Name ?? plan.HotelNameSnapshot,
                plan.HotelPriceSnapshot,
                plan.Hotel?.Images.FirstOrDefault(i => i.IsMain)?.ImageUrl
                    ?? plan.Hotel?.Images.FirstOrDefault()?.ImageUrl
            );

            var restaurantItems = plan.Restaurants.Select(r => new BudgetPlanRestaurantItem(
                r.RestaurantId,
                r.Restaurant?.Name ?? r.RestaurantNameSnapshot,
                r.EstimatedPriceSnapshot,
                r.Restaurant?.Images.FirstOrDefault(i => i.IsMain)?.ImageUrl
                    ?? r.Restaurant?.Images.FirstOrDefault()?.ImageUrl
            )).ToList();

            var attractionItems = plan.Attractions.Select(a => new BudgetPlanAttractionItem(
                a.AttractionId,
                a.Attraction?.Name ?? a.AttractionNameSnapshot,
                a.TicketPriceSnapshot,
                a.Attraction?.Images.FirstOrDefault(i => i.IsMain)?.ImageUrl
                    ?? a.Attraction?.Images.FirstOrDefault()?.ImageUrl
            )).ToList();

            var totalRestaurantCost = restaurantItems.Sum(r => r.EstimatedPrice);
            var totalAttractionCost = attractionItems.Sum(a => a.TicketPrice);

            return new BudgetPlanResponse(
                plan.Id,
                plan.TotalBudget,
                plan.NumberOfDays,
                plan.HotelBudget,
                plan.RestaurantBudget,
                plan.AttractionBudget,
                hotelItem,
                restaurantItems,
                attractionItems,
                plan.HotelPriceSnapshot,
                totalRestaurantCost,
                totalAttractionCost,
                plan.HotelPriceSnapshot + totalRestaurantCost + totalAttractionCost,
                plan.CreatedAt
            );
        }
    }
}