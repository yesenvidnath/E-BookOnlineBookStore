using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using E_BookOnlineBookStore.Models;

namespace E_BookOnlineBookStore.Controllers.Common
{
    public class OrdersController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public OrdersController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("EBookDatabase");
        }

        public IActionResult Index(string query = null)
        {
            // Assuming you have the customer's UserID stored in session
            int customerUserId = int.Parse(HttpContext.Session.GetString("UserID"));
            DataTable ordersTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sqlQuery = @"
                    SELECT Orders.*, Users.Username AS CustomerName 
                    FROM Orders 
                    JOIN Customers ON Orders.CustomerID = Customers.CustomerID 
                    JOIN Users ON Customers.UserID = Users.UserID 
                    WHERE Customers.UserID = @UserID";

                if (!string.IsNullOrWhiteSpace(query))
                {
                    sqlQuery += " AND Orders.OrderStatus LIKE @Query";
                }

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@UserID", customerUserId);

                    if (!string.IsNullOrWhiteSpace(query))
                    {
                        command.Parameters.AddWithValue("@Query", "%" + query + "%");
                    }

                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(ordersTable);
                }
            }

            return View("~/Views/Account/Customer/Orders.cshtml", ordersTable);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Order newOrder)
        {
            // Ensure only the logged-in customer can create an order
            int customerUserId = int.Parse(HttpContext.Session.GetString("UserID"));

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
                    INSERT INTO Orders (CustomerID, OrderStatus, TotalPrice, CreatedAt, UpdatedAt) 
                    VALUES ((SELECT CustomerID FROM Customers WHERE UserID = @UserID), @OrderStatus, @TotalPrice, GETDATE(), GETDATE())";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", customerUserId);
                    command.Parameters.AddWithValue("@OrderStatus", newOrder.OrderStatus ?? "Pending");
                    command.Parameters.AddWithValue("@TotalPrice", newOrder.TotalPrice);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            return Ok();
        }

        [HttpPost]
        public IActionResult Update([FromBody] Order updatedOrder)
        {
            int customerUserId = int.Parse(HttpContext.Session.GetString("UserID"));

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
                    UPDATE Orders 
                    SET OrderStatus = @OrderStatus, TotalPrice = @TotalPrice, UpdatedAt = GETDATE() 
                    WHERE OrderID = @OrderID AND CustomerID = (SELECT CustomerID FROM Customers WHERE UserID = @UserID)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OrderID", updatedOrder.OrderId);
                    command.Parameters.AddWithValue("@UserID", customerUserId);
                    command.Parameters.AddWithValue("@OrderStatus", updatedOrder.OrderStatus);
                    command.Parameters.AddWithValue("@TotalPrice", updatedOrder.TotalPrice);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            int customerUserId = int.Parse(HttpContext.Session.GetString("UserID"));

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
                    DELETE FROM Orders 
                    WHERE OrderID = @OrderID AND CustomerID = (SELECT CustomerID FROM Customers WHERE UserID = @UserID)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OrderID", id);
                    command.Parameters.AddWithValue("@UserID", customerUserId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            return Ok();
        }
    }
}
