using Microsoft.Extensions.Configuration;
using System;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Xml;
using System.Xml.Linq;

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
                // Create 2 DataTable instances.
                DataTable studentTable = new DataTable("students");
                studentTable.Columns.Add("name");
                studentTable.Columns.Add("id");
                studentTable.Rows.Add("mary", 1);
                studentTable.Rows.Add("amir", 2);

                DataTable departmentTable = new DataTable("department");
                departmentTable.Columns.Add("id");
                departmentTable.Columns.Add("physics");
                departmentTable.Rows.Add(1, "chemistry");
                departmentTable.Rows.Add(2, "maths");

                // Create a DataSet and put both tables in it.
                DataSet set = new DataSet("college");
                set.Tables.Add(studentTable);
                set.Tables.Add(departmentTable);
                // Visualize DataSet.
                Console.WriteLine(PrettyXml(set.GetXml()));
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

    }

    static string PrettyXml(string xml)
    {
        var stringBuilder = new StringBuilder();

        var element = XElement.Parse(xml);

        var settings = new XmlWriterSettings();
        settings.OmitXmlDeclaration = true;
        settings.Indent = true;
        settings.NewLineOnAttributes = true;

        using (var xmlWriter = XmlWriter.Create(stringBuilder, settings))
        {
            element.Save(xmlWriter);
        }

        return stringBuilder.ToString();
    }


}
