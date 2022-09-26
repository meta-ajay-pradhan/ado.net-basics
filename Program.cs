using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace adoDotNetBasics;
public class Program
{
    public static void DisplayReader(SqlDataReader address) {
        while(address.Read()) {
            Console.WriteLine("id: ${0}, place: ${1}", address[0], address[1]);
        }
    }
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
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try {
                    SqlCommand cmd = new SqlCommand("Update accounts set balance = balance-500 where accountnumber = 'Account1'",
                    connection,
                    transaction);
                    cmd.ExecuteNonQuery();

                    cmd = new SqlCommand("Update accounts set balance = balance + 500 where accountnumber = 'Account2'",
                    connection,
                    transaction
                    );
                    cmd.ExecuteNonQuery();
                    transaction.Commit();
                    Console.WriteLine("Transaction Commited");
                } catch (Exception e) {
                    transaction.Rollback();
                    Console.WriteLine(e.Message);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

    }
}
