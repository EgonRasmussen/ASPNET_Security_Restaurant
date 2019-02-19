using DataLayer;
using DataLayer.Entities;
using System.Linq;

namespace ServiceLayer
{
    public class RestaurantService : IRestaurantService
    {
        private readonly AppDbContext _ctx;

        public RestaurantService(AppDbContext ctx)
        {
            ctx.Database.EnsureCreated();
            _ctx = ctx;
        }

       

        public IQueryable<Restaurant> GetRestaurants() 
        {
            return _ctx.Restaurants;
        }

        public IQueryable<Restaurant> GetRestaurantsByName(string name = null)
        {
            return _ctx.Restaurants
                .Where(r => string.IsNullOrEmpty(name) || r.Name.StartsWith(name))
                .OrderBy(r => r.Name);
        }

        public Restaurant GetRestaurantById(int restaurantId)
        {
            return _ctx.Restaurants.Find(restaurantId);
        }

        public Restaurant Update(Restaurant updatedRedstaurant)
        {
            _ctx.Restaurants.Update(updatedRedstaurant);
            
            return updatedRedstaurant;
        }  

        public Restaurant Add(Restaurant newRestaurant)
        {
            _ctx.Restaurants.Add(newRestaurant);
            return newRestaurant;
        }

        public int Commit()
        {
            _ctx.SaveChanges();
            return 0;
        }
    }
}
