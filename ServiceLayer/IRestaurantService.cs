using DataLayer.Entities;
using System.Linq;

namespace ServiceLayer
{
    public interface IRestaurantService
    {
        IQueryable<Restaurant> GetRestaurants();
    }
}
