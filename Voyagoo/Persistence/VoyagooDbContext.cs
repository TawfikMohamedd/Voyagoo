using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Voyagoo.Entities;
using Voyagoo.Entities.Restaurants;

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

    }
}
