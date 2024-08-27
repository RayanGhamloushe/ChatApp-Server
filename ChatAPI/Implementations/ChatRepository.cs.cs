using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Data;
using ChatAPI.DB;
using ChatApplication.Interfaces;
//using ChatApplication.Models;
using ChatAPI.Models;
using Dapper; 

namespace ChatApplication.Implementations
{
    public class ChatRepository : IChatRepository
    {
        

        public async Task AddMessage(Messages message)
        {
            using (var connection = Connection.Get_Connection())
            {
                var query = "INSERT INTO Messages (UserId, MessageText, Timestamp, RoomId) VALUES (@UserId, @MessageText, @Timestamp, @RoomId)";
                await connection.ExecuteAsync(query, new { message.UserId, message.MessageText, message.Timestamp, message.RoomId });
            }


        }
        public async Task<int> GetUserIdByEmail(string email)
        {
            using (var connection = Connection.Get_Connection())
            {
                var query = "SELECT Id FROM Users WHERE Email = @Email";
                return await connection.ExecuteScalarAsync<int>(query, new { Email = email });
            }
        }



        public async Task<IEnumerable<MessageWithUser>> GetAllMessagesAsync()
        {
            using (var connection = Connection.Get_Connection())
            {
                string query = @"
                SELECT m.MessageText, m.Timestamp, u.Name AS UserName, m.RoomId
                FROM Messages m
                JOIN Users u ON m.UserId = u.Id
                ORDER BY m.Timestamp";
                return await connection.QueryAsync<MessageWithUser>(query);
            }
        }
        

        public async Task<IEnumerable<MessageWithUser>> GetMessagesByRoomIdAsync(int roomId)
        {
            using (var connection = Connection.Get_Connection())
            {
                string query = @"
                SELECT m.MessageText, m.Timestamp, u.Name AS UserName, m.RoomId
                FROM Messages m
                JOIN Users u ON m.UserId = u.Id
                WHERE m.RoomId = @RoomId
                ORDER BY m.Timestamp";
                return await connection.QueryAsync<MessageWithUser>(query, new { RoomId = roomId });
            }
        }


        public async Task<IEnumerable<Rooms>> GetAllRoomsAsync()
{
    using (var connection = Connection.Get_Connection())
    {
        string query = "SELECT * FROM Rooms";
        return await connection.QueryAsync<Rooms>(query);
    }
}




    }
}
