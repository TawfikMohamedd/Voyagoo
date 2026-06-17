using Microsoft.EntityFrameworkCore;
using Voyagoo.Abstractions;
using Voyagoo.Contracts.Home;
using Voyagoo.Entities.Attractions;
using Voyagoo.Entities.Hotels;
using Voyagoo.Entities.Restaurants;
using Voyagoo.Persistence;

namespace Voyagoo.Services
{
    public class HomeService(VoyagooDbContext context) : IHomeService
    {
        private readonly VoyagooDbContext _context = context;

        public async Task<Result<GetHomeResponse>> GetHomeAsync(CancellationToken cancellationToken = default)
        {
            // ── Section 1: Offers (أعلى 10 فنادق discount) ──
            var offers = await _context.Hotels
                .Where(h => !h.IsDeleted && h.Status == HotelStatus.Active && h.Discount > 0)
                .Include(h => h.Images)
                .OrderByDescending(h => h.Discount)
                .Take(10)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var offerItems = offers.Select(h => new HomeOfferItem(
                h.Id,
                h.Name,
                h.Location,
                h.Rating,
                new[] { h.SinglePrice, h.DoublePrice, h.TriplePrice, h.SuitePrice }.Where(p => p > 0).Min(),
                new[] { h.SinglePrice, h.DoublePrice, h.TriplePrice, h.SuitePrice }.Max(),
                h.Discount,
                h.Images.FirstOrDefault(i => i.IsMain)?.ImageUrl ?? h.Images.FirstOrDefault()?.ImageUrl
            )).ToList();

            // ── Section 2: Recommended (أعلى 3 من كل category) ──
            var topHotels = await _context.Hotels
                .Where(h => !h.IsDeleted && h.Status == HotelStatus.Active)
                .Include(h => h.Images)
                .OrderByDescending(h => h.Rating)
                .Take(3)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var topRestaurants = await _context.Restaurants
                .Where(r => !r.IsDeleted && r.Status == RestaurantStatus.Active)
                .Include(r => r.Images)
                .OrderByDescending(r => r.Rating)
                .Take(3)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var topAttractions = await _context.Attractions
                .Where(a => !a.IsDeleted && a.Status == AttractionStatus.Active)
                .Include(a => a.Images)
                .OrderByDescending(a => a.Rating)
                .Take(3)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var recommended = new RecommendedSection(
                Hotels: topHotels.Select(h => new RecommendedHotelItem(
                    h.Id,
                    h.Name,
                    h.Location,
                    h.Rating,
                    new[] { h.SinglePrice, h.DoublePrice, h.TriplePrice, h.SuitePrice }.Where(p => p > 0).Min(),
                    new[] { h.SinglePrice, h.DoublePrice, h.TriplePrice, h.SuitePrice }.Max(),
                    h.Images.FirstOrDefault(i => i.IsMain)?.ImageUrl ?? h.Images.FirstOrDefault()?.ImageUrl
                )).ToList(),

                Restaurants: topRestaurants.Select(r => new RecommendedRestaurantItem(
                    r.Id,
                    r.Name,
                    r.Address,
                    r.Rating,
                    r.CuisineType.ToString(),
                    r.MinPrice,
                    r.MaxPrice,
                    r.Images.FirstOrDefault(i => i.IsMain)?.ImageUrl ?? r.Images.FirstOrDefault()?.ImageUrl
                )).ToList(),

                Attractions: topAttractions.Select(a => new RecommendedAttractionItem(
                    a.Id,
                    a.Name,
                    a.Location,
                    a.Rating,
                    a.Category.ToString(),
                    a.Images.FirstOrDefault(i => i.IsMain)?.ImageUrl ?? a.Images.FirstOrDefault()?.ImageUrl
                )).ToList()
            );

            // ── Section 3: Available This Week ──
            // IDs اللي اتعرضوا في Recommended عشان نستثنيهم
            var recommendedHotelIds = topHotels.Select(h => h.Id).ToHashSet();
            var recommendedRestaurantIds = topRestaurants.Select(r => r.Id).ToHashSet();

            var availableHotels = await _context.Hotels
                .Where(h => !h.IsDeleted
                         && h.Status == HotelStatus.Active
                         && h.Rating >= 3.5 && h.Rating <= 4.5
                         && !recommendedHotelIds.Contains(h.Id))
                .Include(h => h.Images)
                .OrderByDescending(h => h.Rating)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var availableRestaurants = await _context.Restaurants
                .Where(r => !r.IsDeleted
                         && r.Status == RestaurantStatus.Active
                         && r.Rating >= 3.5 && r.Rating <= 4.5
                         && !recommendedRestaurantIds.Contains(r.Id))
                .Include(r => r.Images)
                .OrderByDescending(r => r.Rating)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            // بنعمل Mix بينهم ونبعت 10 بس
            var availableItems = availableHotels
                .Select(h => new AvailableThisWeekItem(
                    h.Id,
                    h.Name,
                    "Hotel",
                    h.Location,
                    h.Rating,
                    new[] { h.SinglePrice, h.DoublePrice, h.TriplePrice, h.SuitePrice }.Where(p => p > 0).Min(),
                    new[] { h.SinglePrice, h.DoublePrice, h.TriplePrice, h.SuitePrice }.Max(),
                    h.Images.FirstOrDefault(i => i.IsMain)?.ImageUrl ?? h.Images.FirstOrDefault()?.ImageUrl
                ))
                .Concat(availableRestaurants
                .Select(r => new AvailableThisWeekItem(
                    r.Id,
                    r.Name,
                    "Restaurant",
                    r.Address,
                    r.Rating,
                    r.MinPrice,
                    r.MaxPrice,
                    r.Images.FirstOrDefault(i => i.IsMain)?.ImageUrl ?? r.Images.FirstOrDefault()?.ImageUrl
                )))
                .OrderByDescending(x => x.Rating)
                .Take(10)
                .ToList();

            return Result.Success(new GetHomeResponse(offerItems, recommended, availableItems));
        }
    }
}
