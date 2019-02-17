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
    }
}
