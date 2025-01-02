using E_BookOnlineBookStore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace E_BookOnlineBookStore.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            ViewData["AdminName"] = "Admin User"; // Replace with dynamic admin data
            return View("~/Views/Account/Admin/Index.cshtml");
        }

        public IActionResult Books()
        {
            return View("~/Views/Account/Admin/Books.cshtml");
        }

        public IActionResult Customers()
        {
            return View("~/Views/Account/Admin/Customers.cshtml");
        }

        public IActionResult Orders()
        {
            return View("~/Views/Account/Admin/Orders.cshtml");
        }

        public IActionResult Reports()
        {
            return View("~/Views/Account/Admin/Reports.cshtml");
        }        
        
        public IActionResult Login()
        {
            return View("~/Views/Account/Admin/Login.cshtml");
        }
    }
}
