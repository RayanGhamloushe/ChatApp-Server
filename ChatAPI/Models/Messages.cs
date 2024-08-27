namespace ChatAPI.Models
{
    public class Messages
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string MessageText { get; set; }
        public DateTime Timestamp { get; set; }
        public int RoomId { get; set; }  // Add RoomId property
    }
}
