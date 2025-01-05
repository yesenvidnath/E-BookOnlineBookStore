using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace E_BookOnlineBookStore.Controllers
{
    [Route("[controller]")]
    public class AdminLoginController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly PasswordHasher<string> _passwordHasher;

        public AdminLoginController(IConfiguration configuration)
        {
            _configuration = configuration;
            _passwordHasher = new PasswordHasher<string>();
        }

        [HttpPost("/AdminLogin/LoginAdmin")]
        public IActionResult LoginAdmin(string email, string password)
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("EBookDatabase");
                string storedPasswordHash = null;
                string userName = null;
                string role = null;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT PasswordHash, Username, Role FROM Users WHERE Email = @Email";
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
                                role = reader["Role"]?.ToString();
                            }
                        }
                    }
                }

                if (storedPasswordHash == null)
                {
                    return BadRequest(new { message = "Invalid email or password." });
                }

                if (role == "Customer")
                {
                    return BadRequest(new { message = "Access denied. This login is restricted to admins only." });
                }

                if (role == "Admin")
                {
                    var result = _passwordHasher.VerifyHashedPassword(null, storedPasswordHash, password);
                    if (result == PasswordVerificationResult.Success)
                    {
                        // Set session for admin login
                        HttpContext.Session.SetString("AdminID", userName);
                        HttpContext.Session.SetString("AdminUsername", userName);

                        // Redirect to admin dashboard
                        return RedirectToAction("Dashboard", "Admin");
                    }
                    else
                    {
                        return BadRequest(new { message = "Invalid email or password." });
                    }
                }

                return BadRequest(new { message = "Invalid role." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
