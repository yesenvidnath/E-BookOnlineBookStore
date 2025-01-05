using E_BookOnlineBookStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace E_BookOnlineBookStore.Controllers.Admin
{
    public class ManageBooksController : BaseController
    {
        public ManageBooksController(IConfiguration configuration) : base(configuration) { }

        public IActionResult Index(string query = null)
        {
            try
            {
                DataTable booksTable = new DataTable();
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string sqlQuery = "SELECT * FROM Books";
                    if (!string.IsNullOrWhiteSpace(query))
                    {
                        sqlQuery += " WHERE Title LIKE @Query OR Author LIKE @Query";
                    }

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        if (!string.IsNullOrWhiteSpace(query))
                        {
                            command.Parameters.AddWithValue("@Query", "%" + query + "%");
                        }

                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        adapter.Fill(booksTable);
                    }
                }

                return View("~/Views/Account/Admin/Books.cshtml", booksTable);
            }
            catch (Exception ex)
            {
                // Log the error (you can replace this with a proper logging mechanism)
                Console.WriteLine($"Error in Index: {ex.Message}");
                return BadRequest(new { message = "An error occurred while fetching books." });
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody] Book newBook)
        {
            if (newBook == null)
            {
                return BadRequest(new { message = "Invalid book data." });
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = "INSERT INTO Books (Title, Author, ISBN, Description, Price, CategoryID, StockQuantity, CreatedAt, UpdatedAt) " +
                                   "VALUES (@Title, @Author, @ISBN, @Description, @Price, @CategoryID, @StockQuantity, GETDATE(), GETDATE())";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Title", newBook.Title ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Author", newBook.Author ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@ISBN", newBook.Isbn ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Description", newBook.Description ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Price", newBook.Price);
                        command.Parameters.AddWithValue("@CategoryID", newBook.CategoryId ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@StockQuantity", newBook.StockQuantity);

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
                return Ok(new { message = "Book created successfully." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Create: {ex.Message}");
                return StatusCode(500, new { message = "An error occurred while creating the book." });
            }
        }

        [HttpPost]
        public IActionResult Update([FromBody] Book updatedBook)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = "UPDATE Books " +
                                   "SET Title = @Title, Author = @Author, ISBN = @ISBN, Description = @Description, " +
                                   "Price = @Price, CategoryID = @CategoryID, StockQuantity = @StockQuantity, UpdatedAt = GETDATE() " +
                                   "WHERE BookID = @BookID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@BookID", updatedBook.BookId);
                        command.Parameters.AddWithValue("@Title", updatedBook.Title);
                        command.Parameters.AddWithValue("@Author", updatedBook.Author ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@ISBN", updatedBook.Isbn ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Description", updatedBook.Description ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Price", updatedBook.Price);
                        command.Parameters.AddWithValue("@CategoryID", updatedBook.CategoryId ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@StockQuantity", updatedBook.StockQuantity);

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
                return Ok(new { message = "Book updated successfully." });
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"SQL Error in Update: {sqlEx.Message}");
                return BadRequest(new { message = "A database error occurred while updating the book." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Update: {ex.Message}");
                return BadRequest(new { message = "An error occurred while updating the book." });
            }
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = "DELETE FROM Books WHERE BookID = @BookID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@BookID", id);

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
                return Ok(new { message = "Book deleted successfully." });
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"SQL Error in Delete: {sqlEx.Message}");
                return BadRequest(new { message = "A database error occurred while deleting the book." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Delete: {ex.Message}");
                return BadRequest(new { message = "An error occurred while deleting the book." });
            }
        }
    }
}
