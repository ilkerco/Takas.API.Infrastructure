using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Takas.Core.Model.Entities;
using Takas.Core.Services.Interfaces;
using Takas.WebApi.Hubs;
using Takas.WebApi.Services.Interfaces;

namespace Takas.WebApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class ChatHubController:Controller
    {
        private readonly IChatService _chatService;
        private readonly IMessageService _messageService;
        private readonly IUserService _userService;
        private readonly IHubContext<ChatHub> _chat;
        public ChatHubController(IChatService chatService, IMessageService messageService, IHubContext<ChatHub> chat,IUserService userService)
        {
            _userService = userService;
            _messageService = messageService;
            _chatService = chatService;
            _chat = chat;
        }
        [HttpPost("sendMessage/{message}/{chatId}")]
        public async Task<IActionResult> SendMessage(string message,int chatId)
        {
            var _message = new Message
            {
                Text = message,
                Timestamp = DateTime.Now,
                Name = _userService.GetCurrentUser(),
                ChatId = chatId
            };
            await _messageService.AddAsync(_message);
            await _messageService.SaveChanges();
            var currentChat = _chatService.Get(chatId);
            await _chat.Clients.Group(currentChat.ChatName).SendAsync("RecieveMessage",message);
            return Ok();

        }
    }
}
