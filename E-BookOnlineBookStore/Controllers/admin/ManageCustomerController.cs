using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using E_BookOnlineBookStore.Models;

namespace E_BookOnlineBookStore.Controllers.Admin
{
    public class ManageCustomerController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public ManageCustomerController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("EBookDatabase");
        }

        public IActionResult Index()
        {
            DataTable customersTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT Users.UserID, Users.Username, Users.Email, Users.Role, Users.FirstName, Users.LastName, Users.PhoneNumber, Users.Address, Users.CreatedAt, Users.UpdatedAt, Customers.CustomerID 
                                FROM Users 
                                LEFT JOIN Customers ON Users.UserID = Customers.UserID 
                                WHERE Users.Role = 'Customer' OR Users.Role = 'Admin'";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(customersTable);
                }
            }

            return View("~/Views/Account/Admin/Customers.cshtml", customersTable);
        }

        [HttpPost]
        public IActionResult Create([FromBody] User newUser)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Insert into Users table
                string insertUserQuery = "INSERT INTO Users (Username, PasswordHash, Email, Role, FirstName, LastName, PhoneNumber, Address, CreatedAt, UpdatedAt) " +
                                         "VALUES (@Username, @PasswordHash, @Email, @Role, @FirstName, @LastName, @PhoneNumber, @Address, GETDATE(), GETDATE()); SELECT SCOPE_IDENTITY();";
                using (SqlCommand command = new SqlCommand(insertUserQuery, connection))
                {
                    command.Parameters.AddWithValue("@Username", newUser.Username);
                    command.Parameters.AddWithValue("@PasswordHash", newUser.PasswordHash);
                    command.Parameters.AddWithValue("@Email", newUser.Email);
                    command.Parameters.AddWithValue("@Role", newUser.Role);
                    command.Parameters.AddWithValue("@FirstName", newUser.FirstName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@LastName", newUser.LastName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@PhoneNumber", newUser.PhoneNumber ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Address", newUser.Address ?? (object)DBNull.Value);

                    int userId = Convert.ToInt32(command.ExecuteScalar());

                    if (newUser.Role == "Customer")
                    {
                        // Insert into Customers table
                        string insertCustomerQuery = "INSERT INTO Customers (UserID) VALUES (@UserID)";
                        using (SqlCommand customerCommand = new SqlCommand(insertCustomerQuery, connection))
                        {
                            customerCommand.Parameters.AddWithValue("@UserID", userId);
                            customerCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
            return Ok();
        }

        [HttpPost]
        public IActionResult Update([FromBody] User updatedUser)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "UPDATE Users " +
                               "SET Username = @Username, Email = @Email, Role = @Role, FirstName = @FirstName, LastName = @LastName, PhoneNumber = @PhoneNumber, Address = @Address, UpdatedAt = GETDATE() " +
                               "WHERE UserID = @UserID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", updatedUser.UserId);
                    command.Parameters.AddWithValue("@Username", updatedUser.Username);
                    command.Parameters.AddWithValue("@Email", updatedUser.Email);
                    command.Parameters.AddWithValue("@Role", updatedUser.Role);
                    command.Parameters.AddWithValue("@FirstName", updatedUser.FirstName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@LastName", updatedUser.LastName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@PhoneNumber", updatedUser.PhoneNumber ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Address", updatedUser.Address ?? (object)DBNull.Value);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Delete from Customers table
                string deleteCustomerQuery = "DELETE FROM Customers WHERE UserID = @UserID";
                using (SqlCommand customerCommand = new SqlCommand(deleteCustomerQuery, connection))
                {
                    customerCommand.Parameters.AddWithValue("@UserID", id);
                    customerCommand.ExecuteNonQuery();
                }

                // Delete from Users table
                string deleteUserQuery = "DELETE FROM Users WHERE UserID = @UserID";
                using (SqlCommand userCommand = new SqlCommand(deleteUserQuery, connection))
                {
                    userCommand.Parameters.AddWithValue("@UserID", id);
                    userCommand.ExecuteNonQuery();
                }
            }
            return Ok();
        }
    }
}
