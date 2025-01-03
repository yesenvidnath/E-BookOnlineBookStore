using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace E_BookOnlineBookStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly Utilities.ComputeSha256Hash _hashUtility;

        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
            _hashUtility = new Utilities.ComputeSha256Hash(); // SHA-256 utility class
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT UserID, PasswordHash, Role FROM Users WHERE Email = @Email";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Email", email);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        string storedPasswordHash = reader["PasswordHash"].ToString();
                        int userId = (int)reader["UserID"];
                        string role = reader["Role"].ToString();

                        // Verify the password
                        if (_hashUtility.Verify(password, storedPasswordHash))
                        {
                            // Create claims for the authenticated user
                            var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, email),
                                new Claim(ClaimTypes.Role, role),
                                new Claim("UserID", userId.ToString())
                            };

                            // Create the claims identity
                            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                            // Sign in with the authentication cookie
                            await HttpContext.SignInAsync(
                                CookieAuthenticationDefaults.AuthenticationScheme,
                                new ClaimsPrincipal(claimsIdentity),
                                new AuthenticationProperties
                                {
                                    IsPersistent = true, // Keep user logged in across sessions
                                    ExpiresUtc = DateTime.UtcNow.AddHours(8) // Cookie expiration time
                                });

                            TempData["SuccessMessage"] = "Login successful!";
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Invalid password.";
                        }
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Email not found.";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
