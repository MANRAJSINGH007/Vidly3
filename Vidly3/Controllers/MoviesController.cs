using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly3.Models;
using Vidly3.ViewModels;

namespace Vidly3.Controllers
{
    public class MoviesController : Controller
    {
        // GET: Movies
        public ActionResult Random()
        {
            var movie = new Movie() { Name = "Shrek" };
            var customers = new List<Customer>
            {
                new Customer {Name = "Customer1" },
                new Customer {Name = "Customer2" }
            };
            var viewModel = new RandomMovieViewModel {
                Movie = movie,
                Customers = customers
            };
            return View(viewModel);
        }

        public ActionResult Edit(int id)
        {
            return Content("id = " + id);
        }

        public ActionResult Index(int? pageIndex, string sortBy)
        {
            if (!pageIndex.HasValue) pageIndex = 1;
            if (String.IsNullOrWhiteSpace(sortBy)) sortBy = "Name";
            return Content(String.Format("pageIndex={0}&sortBy={1}", pageIndex, sortBy));
        }

        //The regular expression here is not a string so we have to repeat the backslash twice
        //[Route("movies/released/{year}/{month: regex(\\d{4}):regex:range(1, 12)}")]
        [Route("movies/released/{year}/{month}")]
        public ActionResult ByReleaseDate(int year, int month)
        {
            return Content(year + "/" + month);
        }       
    }
}