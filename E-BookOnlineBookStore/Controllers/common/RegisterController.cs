using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using E_BookOnlineBookStore.Utilities;


namespace E_BookOnlineBookStore.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ComputeSha256Hash _sha256Hash;

        public RegisterController(IConfiguration configuration)
        {
            _configuration = configuration;
            _sha256Hash = new ComputeSha256Hash(); // Instantiate the SHA-256 utility
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View("~/Views/Home/Register.cshtml");
        }

        [HttpPost]
        public IActionResult RegisterUser(string firstName, string lastName, string phoneNumber,
                                         string address, string password, string[] genres,
                                         string preferredFormat, string readingFrequency)
        {
            try
            {
                // Get the connection string
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                // Hash the password using SHA-256
                string passwordHash = _sha256Hash.Encrypt(password);

                // Join selected genres into a single string
                string genreString = genres != null ? string.Join(",", genres) : null;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("RegisterUser", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters
                        command.Parameters.AddWithValue("@Username", firstName.ToLower() + lastName.ToLower()); // Example username
                        command.Parameters.AddWithValue("@PasswordHash", passwordHash);
                        command.Parameters.AddWithValue("@Email", $"{firstName.ToLower()}.{lastName.ToLower()}@example.com"); // Example email
                        command.Parameters.AddWithValue("@Role", "Customer"); // Hardcoded role
                        command.Parameters.AddWithValue("@FirstName", firstName);
                        command.Parameters.AddWithValue("@LastName", lastName);
                        command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                        command.Parameters.AddWithValue("@Address", address);
                        command.Parameters.AddWithValue("@Genre", genreString ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@PreferredFormat", preferredFormat ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@ReadingFrequency", readingFrequency ?? (object)DBNull.Value);

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }

                return RedirectToAction("Success");
            }
            catch (Exception ex)
            {
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
