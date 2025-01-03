using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace E_BookOnlineBookStore.Controllers.Admin
{
    public class ManageBooksController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public ManageBooksController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("EBookDatabase");
        }

        public IActionResult Index()
        {
            DataTable booksTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Books";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(booksTable);
                }
            }

            return View("~/Views/Account/Admin/Books.cshtml", booksTable);
        }

    }
}
