using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly3.Models;
using System.Data.Entity;
using Vidly3.ViewModels;

namespace Vidly3.Controllers
{
    public class CustomersController : Controller
    {
        // to access the database we need a DbContext which we will initialize in the constructor
        private ApplicationDbContext _context;

        public CustomersController()
        {
            // this is a disposable object so we need to dispose it also 
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public ActionResult New()
        {
            var membershipTypes = _context.MembershipTypes.ToList();
            var viewModel = new NewCustomerViewModel
            {
                MembershipTypes = membershipTypes
            };
            return View("CustomerForm", viewModel);
        }

        [HttpPost]
        public ActionResult Save(Customer customer)
        {
            if (customer.Id == 0) _context.Customers.Add(customer);
            else
            {
                var customerInDB = _context.Customers.Single(c => c.Id == customer.Id);
                // how to update the customer object
                // opens up security holes in app
                // TryUpdateModel(customerInDB, "", new string[] {"Name", "Email"});

                // Alternative Approach or you can use auto-mapper
                // Another way is to use a DataTransfer Object Dto which will be a datastructure or a subset of the customer class
                customerInDB.Name = customer.Name;
                customerInDB.Birthdate = customer.Birthdate;
                customerInDB.MembershipType = customer.MembershipType;
                customerInDB.IsSubscribedToNewsletter = customer.IsSubscribedToNewsletter;

            }

            // Either all changes get persisted or none gets persisted, they are wrapped in a transaction
            // They will be run at runtine
            _context.SaveChanges();
            return RedirectToAction("Index", "Customers");
        }

        // GET: Customers
        public ActionResult Index()
        {
            // the query would not be executed now, it will be executed when we iterate over the customers object
            //var customers = _context.Customers;
            // but we can immediately execute this query if we call the ToList() method on customers.
            //var customers = _context.Customers.ToList();
            var customers = _context.Customers.Include(c => c.MembershipType).ToList();
            return View(customers);
        }

        public ActionResult Details(int id)
        {
            var customer = _context.Customers.Include(c => c.MembershipType).SingleOrDefault(c => c.Id == id);
            if (customer == null) return HttpNotFound();
            return View(customer);
        }

        public ActionResult Edit(int id)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.Id == id);
            if (customer == null) return HttpNotFound();
            var viewModel = new NewCustomerViewModel
            {
                Customer = customer,
                MembershipTypes = _context.MembershipTypes.ToList()
            };
            // Specify the viewname else the MVC will look for a view with the name Edit
            return View("CustomerForm", viewModel);
        }

        //private IEnumerable<Customer> GetCustomers( )
        //{
        //    return new List<Customer>
        //    {
        //        new Customer { Id = 1, Name = "John Smith" },
        //        new Customer { Id = 2, Name = "Mary Williams" }
        //    };
        //}
    }
}