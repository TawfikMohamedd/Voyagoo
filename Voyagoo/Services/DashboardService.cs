using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Voyagoo.Abstractions;
using Voyagoo.Abstractions.Consts;
using Voyagoo.Contracts.Dashboard;
using Voyagoo.Entities;
using Voyagoo.Persistence;

namespace Voyagoo.Services
{
    public class DashboardService(
        VoyagooDbContext context,
        UserManager<ApplicationUser> userManager) : IDashboardService
    {
        private readonly VoyagooDbContext _context = context;
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        public async Task<Result<GetDashboardResponse>> GetDashboardAsync(CancellationToken cancellationToken = default)
        {
            // Overview
            var totalRestaurants = await _context.Restaurants
                .CountAsync(r => !r.IsDeleted, cancellationToken);

            var totalTourGuides = await _context.TourGuides
                .CountAsync(g => !g.IsDeleted, cancellationToken);

            var totalAttractions = await _context.Attractions
                .CountAsync(a => !a.IsDeleted, cancellationToken);

            var totalHotels = await _context.Hotels
                .CountAsync(h => !h.IsDeleted, cancellationToken);

            var totalUsers = (await _userManager.GetUsersInRoleAsync(DefaultRoles.Member)).Count;

            // Top 3 Restaurants
            var topRestaurants = await _context.Restaurants
                .Where(r => !r.IsDeleted)
                .OrderByDescending(r => r.Rating)
                .Take(3)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            // Top 3 TourGuides
            var topTourGuides = await _context.TourGuides
                .Where(g => !g.IsDeleted)
                .OrderByDescending(g => g.Rating)
                .Take(3)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            // Top 3 Hotels
            var topHotels = await _context.Hotels
                .Where(h => !h.IsDeleted)
                .OrderByDescending(h => h.Rating)
                .Take(3)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            // Recent 3 Users
            var recentUsers = (await _userManager.GetUsersInRoleAsync(DefaultRoles.Member))
                .OrderByDescending(u => u.CreatedAt)
                .Take(3)
                .Select(u => new RecentUserItem(
                    Id: u.Id,
                    FullName: $"{u.FirstName} {u.LastName}",
                    Email: u.Email!,
                    CreatedAt: u.CreatedAt
                )).ToList();

            var response = new GetDashboardResponse(
                Overview: new DashboardOverview(
                    TotalRestaurants: totalRestaurants,
                    TotalTourGuides: totalTourGuides,
                    TotalAttractions: totalAttractions,
                    TotalUsers: totalUsers,
                    TotalHotels: totalHotels
                ),
                TopRestaurants: topRestaurants.Select(r => new TopRestaurantItem(
                    Id: r.Id,
                    Name: r.Name,
                    CuisineType: r.CuisineType.ToString(),
                    Rating: r.Rating,
                    Status: r.Status.ToString()
                )).ToList(),
                TopTourGuides: topTourGuides.Select(g => new TopTourGuideItem(
                    Id: g.Id,
                    Name: g.Name,
                    Rating: g.Rating,
                    Status: g.Status.ToString()
                )).ToList(),
                TopHotels: topHotels.Select(h => new TopHotelItem(
                    Id: h.Id,
                    Name: h.Name,
                    Rating: h.Rating,
                    Status: h.Status.ToString()
                )).ToList(),
                RecentUsers: recentUsers
            );

            return Result.Success(response);
        }
    }
}