using System.Data;
using System.Text;

namespace E_BookOnlineBookStore.Services
{
    public class ReportService
    {
        // Generate HTML content for a books report
        public string GenerateBooksHtmlReport(DataTable reportTable)
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

        // Generate HTML content for an orders report (extendable for other report types)
        public string GenerateOrdersHtmlReport(DataTable reportTable, int month, int year)
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
