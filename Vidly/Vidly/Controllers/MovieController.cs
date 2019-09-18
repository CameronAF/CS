using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
    public class MovieController : Controller
    {
        // GET: Movie
        // Use ActionResult when different execution paths return differnt action results
        // Otherise use the spesific aftion result returned 
        public ActionResult Random()
        {
            // Usualy model content is from a database
            Movie movie = new Movie() { Name = "Shrek!" };
            List<Customer> customers = new List<Customer>
            {
                new Customer { Name = "Customer 1" },
                new Customer { Name = "Customer 2" }
            };

            RandomMovieViewModel viewModel = new RandomMovieViewModel
            {
                Movie = movie,
                Customers = customers
            };

            // Pass data to a view
            // Method 1: using ViewData
            // if the string changes, it must change in the view as well
            ViewData["Movie"] = movie;
            // Method 2: using ViewBag
            // if the name (Movie) changes, it must change in the view as well
            ViewBag.Movie = movie;
            // Method 3: using Model
            // View() store the model here -> new ViewResult().ViewData.Model
            return View(viewModel);

            //return View(movie);
            //return Content("Hello World!");
            //return HttpNotFound();
            //return new EmptyResult();
            //return RedirectToAction("Index", "Home", new { page = 1, sortBy = "name" });
        }

        // 'id' is the default paramater in our default route in RouteConfig.cs
        // Using 'id' as the paramater allows action request paramaters in the URL (i.e .../movie/edit/1)
        // If not in the URL, it can also be in the query string (i.e .../movie/edit?id=1)
        public ActionResult Edit(int id)
        {
            return Content("id=" + id);
        }

        // Action with optional paramaters
        public ActionResult Index(int pageIndex = 1, string sortBy = "name")
        {
            return Content(String.Format("pageIndex={0}&sortBy={1}", pageIndex, sortBy));
        }

        // convetional custom route action
        public ActionResult ByReleaseDate(int year, int month)
        {
            return Content(year + "/" + month);
        }

        // attribute custome route action which support constrains
        // other contrains: min, max, minlength, maxlength, int, float, guid, ect.
        // Google ASP.NET MVC Attribute Route Constraints
        [Route("movie/releaseYear/{year:regex(^[0-9]{4}$)}/{month:range(1,12)}")]
        public ActionResult ByReleaseYear(int year, int month)
        {
            return Content(year + "/" + month);
        }
    }
}