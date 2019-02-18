using DataLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLayer;
using System.Collections.Generic;
using System.Linq;

namespace OdeToFoodWebApp.Pages.Restaurants
{
    public class ListModel : PageModel
    {
        public string Message { get; set; }
        public IEnumerable<Restaurant> Restaurants { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }


        private readonly IRestaurantService _restaurantService;

        public ListModel(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }


        //public void OnGet(string searchTerm)
        public void OnGet()
        {
            //Restaurants = _restaurantService.GetRestaurants().ToList();

            //string searchTerm = HttpContext.Request.QueryString.Value.Split('=').LastOrDefault();  // Uden Model binding

            Restaurants = _restaurantService.GetRestaurantsByName(SearchTerm).ToList();
        }
    }
}