using ChatApp.Models;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Hubs;
public class ChatHub : Hub
{ 
    public void SendChatMessage(ChatMessage message)
    {
        Clients.All.SendAsync("ChatMessageReceived", message);
    }

    public void UserSignedIn(UserSignedIn user)
    {
        var userInfo = new UserSignedIn
        {
            Login = user.Login
        };

        Clients.All.SendAsync("UserSignedIn", userInfo);
    }
}
