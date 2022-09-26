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
                //Select Command
                SqlCommand select = new SqlCommand("select id, place from useraddress", connection);
                int id;
                connection.Open();
                Console.WriteLine("Initial Table Data:");
                DisplayReader(select.ExecuteReader());
                connection.Close();
                //Insert
                SqlCommand insertCommand = new SqlCommand() {
                    CommandText = "spAddAddress",
                    Connection = connection,
                    CommandType = CommandType.StoredProcedure
                };
                SqlParameter place = new SqlParameter {
                    ParameterName = "@place",
                    SqlDbType = SqlDbType.VarChar,
                    Value = "Mumbai",
                    Direction = ParameterDirection.Input
                };
                SqlParameter ids = new SqlParameter {
                    ParameterName = "@id",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                };
                insertCommand.Parameters.Add(place);
                insertCommand.Parameters.Add(ids);
                connection.Open();
                insertCommand.ExecuteNonQuery();
                Console.WriteLine("\nTable after insertion");
                DisplayReader(select.ExecuteReader());
                connection.Close();
                string? idTemp = ids.Value!.ToString();
                if(idTemp == null) {
                    throw new Exception("Failed");
                }
                id = int.Parse(idTemp);
                Console.WriteLine(id);

                //Update
                
                SqlCommand updateCommand = new SqlCommand (){
                    CommandText = "spUpdateAddress",
                    Connection = connection,
                    CommandType = CommandType.StoredProcedure
                };

                updateCommand.Parameters.AddWithValue("@place","Bombay");
                updateCommand.Parameters.AddWithValue("@id", id);

                connection.Open();
                Console.WriteLine("\nTable after updation");
                updateCommand.ExecuteNonQuery();
                DisplayReader(select.ExecuteReader());
                connection.Close();



                //Delete
                SqlCommand deleteCommand = new SqlCommand(){
                    CommandText = "spDeleteAddress",
                    Connection = connection,
                    CommandType = CommandType.StoredProcedure
                };

                deleteCommand.Parameters.AddWithValue("@id", id);

                connection.Open();
                Console.WriteLine("\nTable after deletion");
                deleteCommand.ExecuteNonQuery();
                DisplayReader(select.ExecuteReader());
                connection.Close();
                
                
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

    }
}
