using E_BookOnlineBookStore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace E_BookOnlineBookStore.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            // Pass dynamic customer name
            ViewData["CustomerName"] = "John Doe"; // Replace with the actual customer's name from your database

            // Explicitly specify the view's path
            return View("~/Views/Account/Customer/Index.cshtml");
        }

        public IActionResult Orders()
        {
            // Explicitly specify the Orders view's path
            return View("~/Views/Account/Customer/Orders.cshtml");
        }

        public IActionResult Profile()
        {
            // Explicitly specify the Profile view's path
            return View("~/Views/Account/Customer/Profile.cshtml");
        }

      /*  public IActionResult Settings()
        {
            // Explicitly specify the Settings view's path
            return View("~/Views/Account/Customer/Settings.cshtml");
        }*/
    }
}
