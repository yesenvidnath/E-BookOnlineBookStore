using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace E_BookOnlineBookStore.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly PasswordHasher<string> _passwordHasher;

        public RegisterController(IConfiguration configuration)
        {
            _configuration = configuration;
            _passwordHasher = new PasswordHasher<string>(); // Instantiate the password hasher
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View("~/Views/Home/Register.cshtml");
        }

        [HttpPost]
        public IActionResult RegisterUser(string firstName, string lastName, string phoneNumber,
                                          string address, string email, string password,
                                          string[] genres, string preferredFormat,
                                          string readingFrequency)
        {
            try
            {
                // Get the connection string
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                // Hash the password using ASP.NET Core's PasswordHasher
                string passwordHash = _passwordHasher.HashPassword(null, password);

                // Join selected genres into a single string
                string genreString = genres != null ? string.Join(",", genres) : null;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("RegisterUser", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        // Add parameters
                        command.Parameters.AddWithValue("@Username", email.Split('@')[0]); // Example: username from email prefix
                        command.Parameters.AddWithValue("@PasswordHash", passwordHash);
                        command.Parameters.AddWithValue("@Email", email); // Use the email provided by the user
                        command.Parameters.AddWithValue("@Role", "Customer"); // Hardcoded as Customer
                        command.Parameters.AddWithValue("@FirstName", firstName);
                        command.Parameters.AddWithValue("@LastName", lastName);
                        command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                        command.Parameters.AddWithValue("@Address", address);
                        command.Parameters.AddWithValue("@Genre", genreString ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@PreferredFormat", preferredFormat ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@ReadingFrequency", readingFrequency ?? (object)DBNull.Value);

                        // Open connection and execute
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }

                return RedirectToAction("Success");
            }
            catch (Exception ex)
            {
                // Handle errors
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult Success()
        {
            return View("~/Views/Home/RegisterSuccess.cshtml");
        }
    }
}
