using Microsoft.AspNetCore.Mvc;
using ChatAPI.DB;
using System.Threading.Tasks;
using ChatAPI.Interfaces;
using ChatAPI.Models;
using ChatApplication.Interfaces;

namespace ChatAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly IChatRepository _chatRepository;

        public MessageController(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        [HttpPost]
        public async Task<IActionResult> AddMessage([FromBody] Messages message)
        {
            await _chatRepository.AddMessage(message);
            return Ok(new { success = true, message = "Message saved successfully" });
        }


        [HttpGet("history")]
        public async Task<IActionResult> GetAllMessages()
        {
            try
            {
                var messages = await _chatRepository.GetAllMessagesAsync();
                return Ok(messages);
            }
            catch (Exception ex)
            {
                // Log the exception (logging not shown here for brevity)
                return StatusCode(500, new { success = false, message = "An error occurred while fetching messages.", error = ex.Message });
            }
        }


        [HttpGet("rooms")]
        public async Task<IActionResult> GetRooms()
        {
            var rooms = await _chatRepository.GetAllRoomsAsync();
            return Ok(rooms);
        }


        [HttpGet("rooms/{roomId}")]
        public async Task<IActionResult> GetMessagesByRoomId(int roomId)
        {
            try
            {
                var messages = await _chatRepository.GetMessagesByRoomIdAsync(roomId);
                if (messages == null || !messages.Any())
                {
                    return NotFound(new { success = false, message = "No messages found for this room." });
                }
                return Ok(messages);
            }
            catch (Exception ex)
            {
                // Log the exception (logging not shown here for brevity)
                return StatusCode(500, new { success = false, message = "An error occurred while fetching messages.", error = ex.Message });
            }
        }
    }
}
