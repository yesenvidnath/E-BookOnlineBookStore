// Controllers/common/BooksController.cs
using E_BookOnlineBookStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace E_BookOnlineBookStore.Controllers.Common
{
    public class BooksController : Controller
    {
        private readonly IConfiguration _configuration;

        public BooksController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("/Books/GetAllBooks")]
        public IActionResult GetAllBooks(int? bookID = null)
        {
            try
            {
                var booksWithCategories = new List<dynamic>();

                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("EBookDatabase")))
                {
                    string query = @"SELECT 
                                        b.BookID, 
                                        b.Title, 
                                        b.Author, 
                                        ISNULL(b.BookImage, 'https://via.placeholder.com/300x200') AS BookImage,
                                        b.ISBN, 
                                        ISNULL(b.Description, 'No description available') AS Description,
                                        b.Price, 
                                        b.StockQuantity, 
                                        ISNULL(c.CategoryName, 'Uncategorized') AS CategoryName
                                    FROM Books b
                                    LEFT JOIN Categories c ON b.CategoryID = c.CategoryID";

                    if (bookID.HasValue)
                    {
                        query += " WHERE b.BookID = @BookID";
                    }

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        if (bookID.HasValue)
                        {
                            command.Parameters.AddWithValue("@BookID", bookID.Value);
                        }

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                booksWithCategories.Add(new
                                {
                                    BookID = reader["BookID"],
                                    Title = reader["Title"],
                                    Author = reader["Author"],
                                    BookImage = reader["BookImage"],
                                    ISBN = reader["ISBN"],
                                    Description = reader["Description"],
                                    Price = reader["Price"],
                                    StockQuantity = reader["StockQuantity"],
                                    CategoryName = reader["CategoryName"]
                                });
                            }
                        }
                    }
                }

                return Json(booksWithCategories);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("/Books/GetBookDetails")]
        public IActionResult GetBookDetails(int bookID)
        {
            return GetAllBooks(bookID);
        }
    }
}
