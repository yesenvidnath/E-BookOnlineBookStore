using E_BookOnlineBookStore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace E_BookOnlineBookStore.Controllers.common
{
    public class OrdersController : Controller
    {
        public IActionResult Index()
        {
            // TODO: Pass any data required by the _ProductPanel or _FilterPanel
            return View();
        }
    }
}
