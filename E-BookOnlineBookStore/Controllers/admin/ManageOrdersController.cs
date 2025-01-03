using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using E_BookOnlineBookStore.Models;

namespace E_BookOnlineBookStore.Controllers.Admin
{
    public class ManageOrdersController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public ManageOrdersController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("EBookDatabase");
        }

        public IActionResult Index()
        {
            DataTable ordersTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Orders";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(ordersTable);
                }
            }

            return View("~/Views/Account/Admin/Orders.cshtml", ordersTable);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Order newOrder)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Orders (CustomerID, OrderStatus, TotalPrice, CreatedAt, UpdatedAt) " +
                               "VALUES (@CustomerID, @OrderStatus, @TotalPrice, GETDATE(), GETDATE())";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CustomerID", newOrder.CustomerId);
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
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "UPDATE Orders " +
                               "SET CustomerID = @CustomerID, OrderStatus = @OrderStatus, TotalPrice = @TotalPrice, UpdatedAt = GETDATE() " +
                               "WHERE OrderID = @OrderID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OrderID", updatedOrder.OrderId);
                    command.Parameters.AddWithValue("@CustomerID", updatedOrder.CustomerId);
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
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Orders WHERE OrderID = @OrderID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OrderID", id);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            return Ok();
        }
    }
}
