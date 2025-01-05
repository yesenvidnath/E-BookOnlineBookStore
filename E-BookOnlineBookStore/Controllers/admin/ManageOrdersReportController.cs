using E_BookOnlineBookStore.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;

namespace E_BookOnlineBookStore.Controllers.Admin
{
    public class ManageOrdersReportController : BaseController
    {
        private readonly ReportService _reportService;

        public ManageOrdersReportController(IConfiguration configuration) : base(configuration)
        {
            _reportService = new ReportService();
        }

        // Action to display the Orders Report with Pie Chart
        public IActionResult OrdersReport(int? month, int? year)
        {
            if (month == null || year == null)
            {
                // Default to the current month and year if not selected
                month = DateTime.Now.Month;
                year = DateTime.Now.Year;
            }

            DataTable reportTable = GetOrderData((int)month, (int)year);

            ViewBag.Month = month;
            ViewBag.Year = year;

            return View("~/Views/Account/Admin/OrdersReport.cshtml", reportTable);
        }

        // Action to generate and download the Orders HTML report
        public IActionResult GenerateHtmlReport(int month, int year)
        {
            DataTable reportTable = GetOrderData(month, year);

            // Delegate HTML generation to the ReportService
            string htmlContent = _reportService.GenerateOrdersHtmlReport(reportTable, month, year);
            byte[] fileBytes = Encoding.UTF8.GetBytes(htmlContent);

            return File(fileBytes, "text/html", $"OrdersReport_{month}_{year}.html");
        }

        // Fetch order data for the selected month and year
        private DataTable GetOrderData(int month, int year)
        {
            DataTable reportTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sqlQuery = @"
                    SELECT 
                        OrderStatus, 
                        COUNT(*) AS OrderCount 
                    FROM Orders
                    WHERE MONTH(CreatedAt) = @Month AND YEAR(CreatedAt) = @Year
                    GROUP BY OrderStatus";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@Month", month);
                    command.Parameters.AddWithValue("@Year", year);

                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(reportTable);
                }
            }
            return reportTable;
        }
    }
}
