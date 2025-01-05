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
                string connectionString = _configuration.GetConnectionString("EBookDatabase");
                string storedPasswordHash = null;
                string userName = null;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT PasswordHash, Username FROM Users WHERE Email = @Email";
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
                    return Ok(new { message = $"Welcome back, {userName}!" });
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
    }
}