using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ChatHub : Hub
{
    private static readonly Dictionary<string, (string User, string RoomId)> connectedClients = new Dictionary<string, (string User, string RoomId)>();

  
    public async Task JoinRoom(string user, int currentRoomId)
    {
        string roomIdString = currentRoomId.ToString(); // Convert int to string

        try
        {
            // Add the connection to the group (room)
            await Groups.AddToGroupAsync(Context.ConnectionId, roomIdString);

            // Update the connectedClients dictionary with integer room ID
            connectedClients[Context.ConnectionId] = (user, roomIdString);

            // Notify others in the room that a new user has joined
            await Clients.Group(roomIdString).SendAsync("ReceiveMessage", "", $"{user} has joined the room.");

           // await Clients.Group(roomIdString).SendAsync("ReceiveMessage", null, $"{user} has joined the room.");



            Console.WriteLine($"User {user} with Connection ID {Context.ConnectionId} joined room {currentRoomId}.");
        }
        catch (Exception ex)
        {
            // Log the error message
            Console.WriteLine($"Error in JoinRoom: {ex.Message}");
            throw; // Optionally rethrow or handle the exception
        }
    }



    // Send a message to a specific room
    public async Task SendMessage(string user, string message, int currentRoomId)

    {
        string roomIdString = currentRoomId.ToString(); // Convert int to string

        if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(message) || string.IsNullOrEmpty(roomIdString))
        {
            Console.WriteLine("Invalid arguments received in SendMessage.");
            throw new ArgumentException("Invalid arguments");
        }

        if (connectedClients.TryGetValue(Context.ConnectionId, out var clientInfo))
        {
            if (clientInfo.RoomId == roomIdString)
            {
                await Clients.Group(roomIdString).SendAsync("ReceiveMessage", user, message);
            }
            else
            {
                Console.WriteLine($"User {user} tried to send a message to room {roomIdString} but is not a member of that room.");
            }
        }
        else
        {
            Console.WriteLine($"Connection ID {Context.ConnectionId} not found in connected clients.");
        }
    }

  

    // Handle leaving a specific room
    public async Task LeaveRoom(int currentRoomId)
    {


        string roomIdString = currentRoomId.ToString(); // Convert int to string
        if (connectedClients.TryGetValue(Context.ConnectionId, out var clientInfo))
        {
            if (clientInfo.RoomId == roomIdString)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomIdString);
                connectedClients.Remove(Context.ConnectionId);
                await Clients.Group(roomIdString).SendAsync("ReceiveMessage", "", $"{clientInfo.User} has left the room.");
            }
        }
    }



    // Override OnDisconnectedAsync to handle user disconnection
    public override async Task OnDisconnectedAsync(Exception exception)
    {
        if (connectedClients.TryGetValue(Context.ConnectionId, out var clientInfo))
        {
            await Clients.Group(clientInfo.RoomId).SendAsync("ReceiveMessage", "", $"{clientInfo.User} has disconnected.");
            connectedClients.Remove(Context.ConnectionId);
        }
        await base.OnDisconnectedAsync(exception);
    }
}
