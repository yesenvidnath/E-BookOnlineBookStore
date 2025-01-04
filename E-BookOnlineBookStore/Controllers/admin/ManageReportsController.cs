using E_BookOnlineBookStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;

namespace E_BookOnlineBookStore.Controllers.Admin
{
    public class ManageReportsController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public ManageReportsController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("EBookDatabase");
        }

        // Action to display the report in the view
        public IActionResult Index()
        {
            DataTable reportTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sqlQuery = @"
                    SELECT 
                        b.Title, 
                        b.Author, 
                        c.CategoryName, 
                        b.Price, 
                        b.StockQuantity 
                    FROM Books b
                    LEFT JOIN Categories c ON b.CategoryID = c.CategoryID";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(reportTable);
                }
            }

            return View("~/Views/Account/Admin/Reports.cshtml", reportTable);
        }

        // Action to generate and download the HTML report
        public IActionResult GenerateHtmlReport()
        {
            DataTable reportTable = GetReportData();

            string htmlContent = GenerateHtmlContent(reportTable);
            byte[] fileBytes = Encoding.UTF8.GetBytes(htmlContent);

            return File(fileBytes, "text/html", "BooksReport.html");
        }

        // Fetch report data
        private DataTable GetReportData()
        {
            DataTable reportTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sqlQuery = @"
                    SELECT 
                        b.Title, 
                        b.Author, 
                        c.CategoryName, 
                        b.Price, 
                        b.StockQuantity 
                    FROM Books b
                    LEFT JOIN Categories c ON b.CategoryID = c.CategoryID";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(reportTable);
                }
            }
            return reportTable;
        }

        // Generate HTML content for the report
        private string GenerateHtmlContent(DataTable reportTable)
        {
            StringBuilder htmlContent = new StringBuilder();

            htmlContent.Append("<html><head><title>Books Report</title></head><body>");
            htmlContent.Append("<h1>Books Report</h1>");
            htmlContent.Append("<table border='1' style='width: 100%; border-collapse: collapse;'>");
            htmlContent.Append("<tr><th>Title</th><th>Author</th><th>Category</th><th>Price</th><th>Stock Quantity</th></tr>");

            foreach (DataRow row in reportTable.Rows)
            {
                htmlContent.Append("<tr>");
                htmlContent.Append($"<td>{row["Title"]}</td>");
                htmlContent.Append($"<td>{row["Author"]}</td>");
                htmlContent.Append($"<td>{row["CategoryName"]}</td>");
                htmlContent.Append($"<td>{row["Price"]:C}</td>");
                htmlContent.Append($"<td>{row["StockQuantity"]}</td>");
                htmlContent.Append("</tr>");
            }

            htmlContent.Append("</table>");
            htmlContent.Append("</body></html>");

            return htmlContent.ToString();
        }
    }
}
