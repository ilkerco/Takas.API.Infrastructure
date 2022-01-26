using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Takas.WebApi.Models
{
    public class ChatResponseModel
    {
        public int Id { get; set; }
        public int? SuggestedProductId { get; set; }
        public int TargetProductId { get; set; }
        public string ChatName { get; set; }
        public string ToId { get; set; }
        public string FromId { get; set; }
        public List<MessagesResponseModel> Messages { get; set; }
        public ChatResponseModel()
        {
            Messages = new List<MessagesResponseModel>();
        }

    }
    public class MessagesResponseModel
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
