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
        public async Task JoinRoom(string roomName)
        {

            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
            //await Clients.Group(roomName).SendAsync("RecieveMessage", "ilkerr");
           
        }
        
    }
}
