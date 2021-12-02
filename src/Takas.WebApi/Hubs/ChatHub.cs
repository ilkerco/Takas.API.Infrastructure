using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Takas.WebApi.Hubs
{
    public class ChatHub:Hub
    {
        public string GetConnectionId() => Context.ConnectionId;
        public Task JoinRoom(string roomName)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        }
    }
}
