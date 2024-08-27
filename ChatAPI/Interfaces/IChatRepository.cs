using System.Collections.Generic;
using System.Threading.Tasks;
using ChatAPI.Models;

namespace ChatApplication.Interfaces
{
    public interface IChatRepository
    {
        Task AddMessage(Messages message);
        Task<int> GetUserIdByEmail(string email);
        Task<IEnumerable<MessageWithUser>> GetAllMessagesAsync();
        Task<IEnumerable<MessageWithUser>> GetMessagesByRoomIdAsync(int roomId);
        Task<IEnumerable<Rooms>> GetAllRoomsAsync();
    }
}
