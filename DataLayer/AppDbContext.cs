using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataLayer
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Restaurant> Restaurants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Restaurant>().HasData(
                new Restaurant { Id = 1, Name = "Scott's Pizza", Loacation = "Maryland", Cuisine = CuisineType.Italian },
                new Restaurant { Id = 2, Name = "Cinnamon Club", Loacation = "London", Cuisine = CuisineType.Italian },
                new Restaurant { Id = 3, Name = "La Costa", Loacation = "California", Cuisine = CuisineType.Mexican }
           );
        }
    }

}
