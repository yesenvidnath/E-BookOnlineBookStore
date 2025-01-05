using E_BookOnlineBookStore.Models;
using E_BookOnlineBookStore.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;

namespace E_BookOnlineBookStore.Controllers.Admin
{
    public class ManageReportsController : BaseController
    {
        private readonly ReportService _reportService;

        public ManageReportsController(IConfiguration configuration) : base(configuration)
        {
            _reportService = new ReportService();
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
        public IActionResult GenerateHtmlReport()
        {
            DataTable reportTable = GetReportData();

            // Use the ReportService to generate HTML content
            string htmlContent = _reportService.GenerateBooksHtmlReport(reportTable);
            byte[] fileBytes = Encoding.UTF8.GetBytes(htmlContent);

            return File(fileBytes, "text/html", "BooksReport.html");
        }
    }
}
