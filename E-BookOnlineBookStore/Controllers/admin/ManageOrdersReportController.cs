using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;

namespace E_BookOnlineBookStore.Controllers.Admin
{
    public class ManageOrdersReportController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public ManageOrdersReportController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("EBookDatabase");
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

            // Generate HTML content with the embedded pie chart
            string htmlContent = GenerateHtmlContentWithChart(reportTable, month, year);
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

        // Generate HTML content with the pie chart as an embedded image
        private string GenerateHtmlContentWithChart(DataTable reportTable, int month, int year)
        {
            StringBuilder htmlContent = new StringBuilder();

            // Add basic HTML structure
            htmlContent.Append("<html><head><title>Orders Report</title>");
            htmlContent.Append("<script src=\"https://cdn.jsdelivr.net/npm/chart.js\"></script>");
            htmlContent.Append("</head><body>");
            htmlContent.Append($"<h1>Orders Report for {new DateTime(year, month, 1).ToString("MMMM yyyy")}</h1>");

            // Add the data table
            htmlContent.Append("<table border='1' style='width: 100%; border-collapse: collapse;'>");
            htmlContent.Append("<tr><th>Order Status</th><th>Order Count</th></tr>");
            foreach (DataRow row in reportTable.Rows)
            {
                htmlContent.Append("<tr>");
                htmlContent.Append($"<td>{row["OrderStatus"]}</td>");
                htmlContent.Append($"<td>{row["OrderCount"]}</td>");
                htmlContent.Append("</tr>");
            }
            htmlContent.Append("</table>");

            // Add a styled div for the pie chart
            htmlContent.Append("<h2>Orders Distribution</h2>");
            htmlContent.Append("<div style=\"width: 300px; height: 300px; margin: auto;\">");
            htmlContent.Append("<canvas id='ordersPieChart'></canvas>");
            htmlContent.Append("</div>");

            // Add Chart.js script to render the pie chart
            htmlContent.Append("<script>");
            htmlContent.Append("const ctx = document.getElementById('ordersPieChart').getContext('2d');");
            htmlContent.Append("const chartData = {");
            htmlContent.Append("    labels: [");
            foreach (DataRow row in reportTable.Rows)
            {
                htmlContent.Append($"'{row["OrderStatus"]}',");
            }
            htmlContent.Append("    ],");
            htmlContent.Append("    datasets: [{");
            htmlContent.Append("        label: 'Orders by Status',");
            htmlContent.Append("        data: [");
            foreach (DataRow row in reportTable.Rows)
            {
                htmlContent.Append($"{row["OrderCount"]},");
            }
            htmlContent.Append("        ],");
            htmlContent.Append("        backgroundColor: [");
            htmlContent.Append("'rgba(255, 99, 132, 0.6)', 'rgba(54, 162, 235, 0.6)', 'rgba(255, 206, 86, 0.6)', 'rgba(75, 192, 192, 0.6)'],");
            htmlContent.Append("        borderColor: [");
            htmlContent.Append("'rgba(255, 99, 132, 1)', 'rgba(54, 162, 235, 1)', 'rgba(255, 206, 86, 1)', 'rgba(75, 192, 192, 1)'],");
            htmlContent.Append("        borderWidth: 1");
            htmlContent.Append("    }]");
            htmlContent.Append("};");
            htmlContent.Append("new Chart(ctx, { type: 'pie', data: chartData });");
            htmlContent.Append("</script>");

            // Close HTML structure
            htmlContent.Append("</body></html>");

            return htmlContent.ToString();
        }
    }
}
