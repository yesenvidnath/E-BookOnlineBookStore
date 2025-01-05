using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

public abstract class BaseController : Controller
{
    protected readonly IConfiguration _configuration;
    protected readonly string _connectionString;

    protected BaseController(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = configuration.GetConnectionString("EBookDatabase");
    }

    protected DataTable ExecuteQuery(string query, SqlParameter[] parameters = null)
    {
        DataTable table = new DataTable();
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                if (parameters != null)
                    command.Parameters.AddRange(parameters);

                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(table);
            }
        }
        return table;
    }
}
