using System.Data;
using System.Data.SqlClient;

namespace ChatApplication.DB
{
    public class Connection
    {
        #region members 
        private static string connectionString;
        #endregion

        #region Porperties
        public static string connectionsString { get; set; }
       
        #endregion

        #region Constructor 
   
        public Connection()
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get Instence For The Sql Connection String 
        /// </summary>
        /// <returns></returns>
        public static IDbConnection Get_Connection()
        {

            IDbConnection dbConnection = null;
            dbConnection = new SqlConnection(connectionsString);

            dbConnection.Open();
            return dbConnection;
        }
        #endregion

    }
}
