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

            string connectionString = $@"Data Source=${serverName};
                                    Initial Catalog=${database};
                                    User ID=${username};
                                    Password=${password}";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                Console.WriteLine("Connected to database successfully!");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

    }
}
