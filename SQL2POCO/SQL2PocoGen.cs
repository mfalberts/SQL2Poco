using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SQL2POCO
{
    public class SQL2PocoGen
    {
         static  IServiceProvider ServiceProvider { get; set; }

        // todo:
        // 1. Build SqlConnection,SqlCommand 
        // 2. get the schema from the reader.
        // 3. build the POCO<ClassName>
        // 4. build the Get<ClassName>
        // 5. build the Set<ClassName>
        static  SQL2PocoGen() { }
        static public void Generate(IServiceProvider serviceProvider, POCOConfig pocoConfig)
        {
            ServiceProvider = serviceProvider;
            var configuration = ServiceProvider.GetRequiredService<IConfiguration>();
            string SQLConnection = configuration.GetConnectionString("SQLConnection");
            using (var connection = new SqlConnection(SQLConnection))
            {
                connection.Open();
                SqlCommand sqlCommand = new SqlCommand(pocoConfig.sqlCommand);
                sqlCommand.Connection = connection;
                using (var sqlReader = sqlCommand.ExecuteReader())
                {
                    DataTable schemaTable = sqlReader.GetSchemaTable();
                    BuildPoco(pocoConfig.className, schemaTable);
                }
            }
        }

        private static string BuildPoco(string className, DataTable schemaTable)
        {
            string classString =
            $@"public class {className}
            {{
            ";
            foreach (DataRow dr in schemaTable.Rows)
            {
                string dataType = dr["DataType"].ToString();
                char allowNullChar = ((bool) dr["AllowDBNull"] && dataType != "System.String") ? '?' : ' ';
                classString += $"public {dr["DataType"].ToString()}{allowNullChar}  {dr["ColumnName"].ToString()} {{get; set; }} \n";


            }
            classString += "}";
            return classString;
        }
    }

}
