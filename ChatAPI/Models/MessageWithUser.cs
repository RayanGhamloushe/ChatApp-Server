namespace ChatAPI.Models
{
    public class MessageWithUser
    {
        public string MessageText { get; set; }
        public DateTime Timestamp { get; set; }
        public string UserName { get; set; }
        public int RoomId { get; set; }  // Add this property
    }
}
