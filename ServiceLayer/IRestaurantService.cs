using DataLayer.Entities;
using System.Collections;
using System.Linq;

namespace ServiceLayer
{
    public interface IRestaurantService
    {
        IQueryable<Restaurant> GetRestaurants();
        IQueryable<Restaurant> GetRestaurantsByName(string name = null);
        Restaurant GetRestaurantById(int restaurantId);
        Restaurant Update(Restaurant updatedRedstaurant);
        Restaurant Add(Restaurant newRestaurant);
        Restaurant Delete(int id);
        int Commit();
    }
}
