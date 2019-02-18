using DataLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceLayer;
using System.Collections.Generic;

namespace WebApp.Pages.Restaurants
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public Restaurant Restaurant { get; set; }
        public IEnumerable<SelectListItem> Cuisines { get; set; }

        private readonly IRestaurantService _restaurantService;
        private IHtmlHelper _htmlHelper;

        public EditModel(IRestaurantService restaurantService, IHtmlHelper htmlHelper)
        {
            _restaurantService = restaurantService;
            _htmlHelper = htmlHelper;
        }

        public IActionResult OnGet(int restaurantId)
        {
            Cuisines = _htmlHelper.GetEnumSelectList<CuisineType>();

            Restaurant = _restaurantService.GetRestaurantById(restaurantId);

            if (Restaurant == null)
            {
                return RedirectToPage("./NotFound");
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                _restaurantService.Update(Restaurant);
                _restaurantService.Commit();
                return RedirectToPage("./Detail", new { restaurantId = Restaurant.Id });
            }
            Cuisines = _htmlHelper.GetEnumSelectList<CuisineType>();         
            return Page();
        }
    }
}