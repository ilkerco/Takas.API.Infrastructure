using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Takas.Core.Model.Entities;
using Takas.Core.Services.Interfaces;
using Takas.WebApi.Hubs;
using Takas.WebApi.Models;
using Takas.WebApi.Services.Interfaces;

namespace Takas.WebApi.Controllers
{

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class ChatController:Controller
    {
        private readonly IChatService _chatService;
        private readonly IUserService _userService;
        private readonly IMessageService _messageService;
        private readonly IHubContext<ChatHub> _chat;
        public ChatController(IChatService chatService,IMessageService messageService,IHubContext<ChatHub> chat,IUserService userService)
        {
            _messageService = messageService;
            _chatService = chatService;
            _chat = chat;
            _userService = userService;
        }

        [HttpPost("createChat")]
        public async Task<IActionResult> CreateChat([FromBody]CreateChatModel createChatModel)
        {
            try
            {
                var allMessages = new ChatsResponseModel();
                var currentUserId = _userService.GetCurrentUser();
                var chatExist = await  _chatService.GetAll().Include(x => x.Messages).Where(x => x.ToId == createChatModel.ToId &&
                                x.FromId == currentUserId && x.SuggestedProductId == createChatModel.SuggestedProductId).FirstOrDefaultAsync();
                if (chatExist != null)
                {
                    allMessages.ChatId = chatExist.Id;
                    allMessages.ChatName = chatExist.ChatName;
                    foreach(var message in chatExist.Messages)
                    {
                        allMessages.Messages.Add(new MessageResponseModel { 
                        Id = message.Id,
                        Name = message.Name,
                        Text = message.Text,
                        Timestamp = message.Timestamp
                        });
                    }
                    return Ok(allMessages);
                }
                var newChat = new Chat
                {
                    SuggestedProductId = createChatModel.SuggestedProductId,
                    TargetProductId = createChatModel.TargetProductId,
                    ChatName = Guid.NewGuid().ToString(),
                    FromId = currentUserId,
                    ToId = createChatModel.ToId,
                };
                await _chatService.AddAsync(newChat);
                await _chatService.SaveChanges();
                allMessages.ChatId = newChat.Id;
                allMessages.ChatName = newChat.ChatName;
                return Ok(allMessages);
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
           
        }
        [HttpGet("getChat/{id}")]
        public async Task<Chat> GetChat(int id)
        {
            try
            {
                var chat =await _chatService.GetAll().Include(x=>x.Messages).FirstOrDefaultAsync(x=>x.Id == id);
                return chat;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [HttpPost("createMessage")]
        public async Task<bool> CreateMessage(int chatId,string message)
        {
            try
            {
                var _message = new Message {
                    Text = message,
                    Timestamp = DateTime.Now,
                    Name = _userService.GetCurrentUser(),
                    ChatId = chatId
                };
                await _messageService.AddAsync(_message);
                await _messageService.SaveChanges();
                
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

    }
}
