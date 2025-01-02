using E_BookOnlineBookStore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace E_BookOnlineBookStore.Controllers.common
{
    public class BooksController : Controller
    {
        public IActionResult Index()
        {
            // TODO: Pass any data required by the _ProductPanel or _FilterPanel
            return View();
        }

        // Single Book Page
        public IActionResult SingleBook(int bookId)
        {
            // TODO: Fetch book details and reviews from the database using bookId
            // For now, return the view with static content
            return View();
        }
    }
}
