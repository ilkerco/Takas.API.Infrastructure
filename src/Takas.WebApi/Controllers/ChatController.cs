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
        private readonly ITakasDataServices _takasDataServices;
        public ChatController(
            IChatService chatService,
            IMessageService messageService,
            IHubContext<ChatHub> chat,
            IUserService userService,
            ITakasDataServices takasDataServices)
        {
            _messageService = messageService;
            _chatService = chatService;
            _chat = chat;
            _userService = userService;
            _takasDataServices = takasDataServices;
        }

        [HttpPost("sendMessage")]
        public async Task<IActionResult> SendMessage([FromBody]CreateChatModel createChatModel)
        {
            try
            {
                //var allMessages = new ChatsResponseModel();
                var currentUserId = _userService.GetCurrentUser();
                var chatExist = await  _chatService.GetAll().Include(x => x.Messages).Where(x => (x.ToId == currentUserId ||
                                x.FromId == currentUserId) && x.TargetProductId == createChatModel.TargetProductId).FirstOrDefaultAsync();
                if (chatExist != null)
                {
                    await _messageService.AddAsync(new Message
                    {
                        ChatId = chatExist.Id,
                        Name = currentUserId,
                        Text = createChatModel.MessageText,
                        Timestamp = DateTime.Now,
                        
                    });
                    await _messageService.SaveChanges();
                    /*allMessages.ChatId = chatExist.Id;
                    allMessages.ChatName = chatExist.ChatName;
                    foreach(var message in chatExist.Messages)
                    {
                        allMessages.Messages.Add(new MessageResponseModel { 
                        Id = message.Id,
                        Name = message.Name,
                        Text = message.Text,
                        Timestamp = message.Timestamp
                        });
                    }*/
                    var arg = new object[] { currentUserId, createChatModel.MessageText, DateTime.Now };
                    await _chat.Groups.AddToGroupAsync(createChatModel.ConnectionId, chatExist.ChatName);
                    await _chat.Clients.Group(chatExist.ChatName)
                        .SendAsync("ReceiveMessage", currentUserId, createChatModel.MessageText, DateTime.Now);
                    //await _chat.Clients.Group(chatExist.ChatName).SendAsync("ReceiveMessage", "ilker");
                    return Ok();
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
                await _messageService.AddAsync(new Message
                {
                    ChatId = newChat.Id,
                    Name = currentUserId,
                    Text = createChatModel.MessageText,
                    Timestamp = DateTime.Now,

                });
                await _messageService.SaveChanges();
                //allMessages.ChatId = newChat.Id;
                //allMessages.ChatName = newChat.ChatName;
                //ChatHub chatHub = new ChatHub();
                //var gg = chatHub.GetConnectionId();
                
                await _chat.Groups.AddToGroupAsync(createChatModel.ConnectionId, newChat.ChatName);
                await _chat.Clients.Group(newChat.ChatName).SendAsync("ReceiveMessage","ilker");
                //await _chat.Groups.AddToGroupAsync(createChatModel.ToId, newChat.ChatName);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
           
        }
        [HttpPost("createChat")]
        public async Task<IActionResult> CreateChat([FromBody] CreateChatModel createChatModel)
        {
            var currentUserId = _userService.GetCurrentUser();
            var chatExist = await _chatService.GetAll().Include(x => x.Messages).Where(x => (x.ToId == currentUserId ||
                           x.FromId == currentUserId) && x.TargetProductId == createChatModel.TargetProductId).FirstOrDefaultAsync();
            if (chatExist != null)
            {
                return Ok();
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
            return Ok();
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
        [HttpGet("getUserChats")]
        public async Task<IActionResult> GetUserChats()
        {
            try
            {
                var data = await _takasDataServices.GetChatsByUser();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpGet("getUserSingleChat/{targetProductId}")]
        public async Task<IActionResult> GetUserSingleChat(int targetProductId)
        {
            try
            {
                var data = await _takasDataServices.GetSingleChat(targetProductId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest();
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
