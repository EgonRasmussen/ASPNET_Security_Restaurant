using DataLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLayer;

namespace WebApp.Pages.Restaurants
{
    public class DeleteModel : PageModel
    {
        [BindProperty]
        public Restaurant Restaurant { get; set; }

        private readonly IRestaurantService _restaurantService;

        public DeleteModel(IRestaurantService restaurantService)
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

        public IActionResult OnPost(int restaurantId)
        {
            var restaurant = _restaurantService.Delete(restaurantId);
            _restaurantService.Commit();

            if (restaurant == null)
            {
                return RedirectToPage("./NotFound");
            }

            TempData["Message"] = $"{restaurant.Name} deleted";
            return RedirectToPage("./List");   
        }
    }
}