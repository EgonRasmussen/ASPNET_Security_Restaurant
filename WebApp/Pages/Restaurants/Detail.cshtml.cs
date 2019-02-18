using DataLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLayer;

namespace WebApp.Pages.Restaurants
{
    public class DetailModel : PageModel
    {
        public Restaurant Restaurant { get; set; }
        [TempData]
        public string Message { get; set; }

        private readonly IRestaurantService _restaurantService;

        public DetailModel(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        public IActionResult OnGet(int restaurantId)
        {
            Restaurant = _restaurantService.GetRestaurantById(restaurantId);

            if (Restaurant == null)
            {
                return RedirectToPage("./NotFound");
            }
            return Page();
        }
    }
}