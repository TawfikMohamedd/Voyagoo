using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Voyagoo.Entities;
using Voyagoo.Entities.Attractions;
using Voyagoo.Entities.Favorites;
using Voyagoo.Entities.Hotels;
using Voyagoo.Entities.Restaurants;
using Voyagoo.Entities.TourGuides;

namespace Voyagoo.Persistence
{
    public class VoyagooDbContext(DbContextOptions<VoyagooDbContext> options) : 
        IdentityDbContext<ApplicationUser,ApplicationRole, string>(options)
    {

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }


        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<RestaurantImage> RestaurantImages { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<RestaurantFeature> RestaurantFeatures { get; set; }
        public DbSet<RestaurantComment> RestaurantComments { get; set; }
        public DbSet<Booking> Bookings { get; set; }



        public DbSet<TourGuide> TourGuides { get; set; }
        public DbSet<TourGuideBooking> TourGuideBookings { get; set; }

        public DbSet<Attraction> Attractions { get; set; }
        public DbSet<AttractionImage> AttractionImages { get; set; }
        public DbSet<Favorite> Favorites { get; set; }





        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<HotelImage> HotelImages { get; set; }
        public DbSet<HotelFeature> HotelFeatures { get; set; }
        public DbSet<HotelFeatureMap> HotelFeatureMaps { get; set; }
        public DbSet<HotelComment> HotelComments { get; set; }
        public DbSet<BookingFeature> BookingFeatures { get; set; }
        public DbSet<HotelBookingFeature> HotelBookingFeatures { get; set; }


    }
}
