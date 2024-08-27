using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Data;
using ChatAPI.DB;
using ChatAPI.Interfaces;
using ChatAPI.Models;
using Dapper;


namespace ChatAPI.Implementations


{
 
        public class UserRepository : IUserRepository
        {


        public async Task<bool> UserExistsAsync(string email)
        {
            try
            {
                using (var connection = Connection.Get_Connection())
                {
                    string query = "SELECT COUNT(1) FROM Users WHERE Email = @Email";
                    var exists = await connection.ExecuteScalarAsync<int>(query, new { Email = email });
                    return exists > 0;
                }
            }
            catch (Exception ex)
            {
                // Log the exception (logging not shown here for brevity)
                throw new Exception("Error checking if user exists.", ex);
            }
        }


        public async Task AddUserAsync(Users user)
        {
            try
            {
                using (var connection = Connection.Get_Connection())
                {
                    string query = "INSERT INTO Users (Name, Email, Password) VALUES (@Name, @Email, @Password)";
                    await connection.ExecuteAsync(query, new { Name = user.Name, Email = user.Email, Password = user.Password });
                }
            }
            catch (Exception ex)
            {
                // Log the exception (logging not shown here for brevity)
                throw new Exception("Error adding user.", ex);
            }
        }
        public async Task<Users> GetUserByEmailAsync(string email)
        {
            try
            {
                using (var connection = Connection.Get_Connection())
                {
                    string query = "SELECT * FROM Users WHERE Email = @Email";
                    return await connection.QuerySingleOrDefaultAsync<Users>(query, new { Email = email });
                }
            }
            catch (Exception ex)
            {
                // Log the exception (logging not shown here for brevity)
                throw new Exception("Error getting user by email.", ex);
            }
        }



    }

    }

