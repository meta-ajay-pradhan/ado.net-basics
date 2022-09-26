using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace adoDotNetBasics;
public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            string serverName = configuration.GetConnectionString("SERVER_NAME");
            string database = configuration.GetConnectionString("DATABASE");
            string username = configuration.GetConnectionString("USERNAME");
            string password = configuration.GetConnectionString("PASSWORD");

            string connectionString = $"Data Source={serverName};Initial Catalog={database};User ID={username};Password={password}";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //Create DataSet
                string query = "Select id,username from Users";
                DataSet ds = new DataSet();
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.Fill(ds);

                DataTable table = ds.Tables[0];

                //Insert
                table.Rows.Add(2, "Nikhil");
                table.Rows.Add(1, "Debjyoti");
                table.Rows.Add(3, "Rakshak");
                table.Rows.Add(4, "Ritik");
                table.Rows.Add(5, "Rahul");
                table.Rows.Add(6, "Alan");
                table.Rows.Add(7, "Karan");

                DataRow[]? rows = table.Select();

                //Delete
                foreach(DataRow row in rows) {
                    if(row["id"].ToString() == "7") {
                        row.Delete();
                        break;
                    }
                }
                
                //Update
                rows = table.Select();

                foreach(DataRow row in rows) {
                    if(row["id"].ToString() == "6") {
                        row["username"] = "Rajeshwar";
                        break;
                    }
                }

                //Select
                rows = table.Select();

                foreach(DataRow row in rows) {
                    Console.WriteLine("id: {0}, name: {1}", row["id"].ToString(), row["username"].ToString());
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw e;
        }

    }
}
