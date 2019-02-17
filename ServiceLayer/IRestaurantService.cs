using DataLayer.Entities;
using System.Linq;

namespace ServiceLayer
{
    public interface IRestaurantService
    {
        IQueryable<Restaurant> GetRestaurants();
        IQueryable<Restaurant> GetRestaurantsByName(string name = null);
    }
}
