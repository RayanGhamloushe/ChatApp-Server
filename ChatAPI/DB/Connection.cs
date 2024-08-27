using System.Data;
using System.Data.SqlClient;

namespace ChatAPI.DB
{
    public class Connection
    {
        public static string connectionsString { get; set; }

        public static IDbConnection Get_Connection()
        {
            IDbConnection dbConnection = new SqlConnection(connectionsString);
            dbConnection.Open();
            return dbConnection;
        }
    }
}
