// Authors Jacob Poor, Ryan Pinkney, Kevin Gutierrez, Tanner Davis
// This is our City View Component

using System;
using System.Linq;
using AuthLab2_RyanPinkney.Models;
using Microsoft.AspNetCore.Mvc;

namespace AuthLab2_RyanPinkney.Components
{
    public class CityViewComponent : ViewComponent
    {

        private ICrashRepository repo { get; set; }

        // Set the contructor
        public CityViewComponent(ICrashRepository temp)
        {
            repo = temp;
        }

        // Grab the information for the repository and decide whay will be returned to the view component
        public IViewComponentResult Invoke()
        {
            // Set the view bag
            ViewBag.SelectedType = RouteData?.Values["cityNames"];

            // Get the data
            var cities = repo.Accidents
                .Where(x => x.county_name != null)
                .Select(x => x.county_name)
                .Distinct()
                .OrderBy(x => x)
                .ToList();


            // Return the team names
            return View(cities);
        }




    }
    






    
}
