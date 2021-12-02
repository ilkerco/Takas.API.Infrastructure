using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Takas.Core.Model.Entities;

namespace Takas.WebApi.Models
{
    public class ChatsResponseModel
    {
        public int ChatId { get; set; }
        public string ChatName { get; set; }
        public List<MessageResponseModel> Messages { get; set; }
        public ChatsResponseModel()
        {
            Messages = new List<MessageResponseModel>();
        }
    }
    public class MessageResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
