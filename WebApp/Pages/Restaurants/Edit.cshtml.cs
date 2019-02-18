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

        public IActionResult OnGet(int? restaurantId)
        {
            Cuisines = _htmlHelper.GetEnumSelectList<CuisineType>();

            if (restaurantId.HasValue)
            {
                Restaurant = _restaurantService.GetRestaurantById(restaurantId.Value);
            }
            else
            {
                Restaurant = new Restaurant();
            }
            

            if (Restaurant == null)
            {
                return RedirectToPage("./NotFound");
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                Cuisines = _htmlHelper.GetEnumSelectList<CuisineType>();
                return Page();
               
            }
            if (Restaurant.Id > 0)
            {
                _restaurantService.Update(Restaurant);
            }
            else
            {
                _restaurantService.Add(Restaurant);
            }
            _restaurantService.Commit();
            TempData["Message"] = "Restaurant saved!";
            return RedirectToPage("./Detail", new { restaurantId = Restaurant.Id });
        }
    }
}