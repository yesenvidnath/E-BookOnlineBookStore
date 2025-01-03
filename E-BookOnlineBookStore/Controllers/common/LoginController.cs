using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace E_BookOnlineBookStore.Controllers
{
    [Route("[controller]")]
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly PasswordHasher<string> _passwordHasher;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
            _passwordHasher = new PasswordHasher<string>();
        }

        [HttpPost("/Login/LoginUser")]
        public IActionResult LoginUser(string email, string password)
        {
            try
            {
                // Existing login logic
                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                string storedPasswordHash = null;
                string userName = null;
                int userId = 0;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT PasswordHash, Username, UserId FROM Users WHERE Email = @Email";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                storedPasswordHash = reader["PasswordHash"]?.ToString();
                                userName = reader["Username"]?.ToString();
                                userId = Convert.ToInt32(reader["UserId"]);
                            }
                        }
                    }
                }

                if (storedPasswordHash == null)
                {
                    return BadRequest(new { message = "Invalid email or password." });
                }

                var result = _passwordHasher.VerifyHashedPassword(null, storedPasswordHash, password);

                if (result == PasswordVerificationResult.Success)
                {
                    HttpContext.Session.SetInt32("UserId", userId);
                    HttpContext.Session.SetString("UserName", userName);

                    // Redirect to the home page or reload the current page
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return BadRequest(new { message = "Invalid email or password." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["LogoutMessage"] = "You have been successfully logged out.";
            return RedirectToAction("Index", "Home");
        }


    }

}

