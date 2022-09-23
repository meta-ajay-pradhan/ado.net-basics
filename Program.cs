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
                string query = @"select productName,price from product";
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader product = command.ExecuteReader();
                while(product.Read()) {
                    Console.WriteLine("Name: {0}     Price: {1}",product.GetValue(0), product.GetValue(1));
                }
                product.Close();

                string query1 = "select max(price) from product";
                SqlCommand command1 = new SqlCommand(query1,connection);
                var price = command1.ExecuteScalar();
                Console.WriteLine("Max Price: " + price.ToString());
                
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

    }
}
