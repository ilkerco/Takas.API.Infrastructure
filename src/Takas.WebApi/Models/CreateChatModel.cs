using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Takas.WebApi.Models
{
    public class CreateChatModel
    {
        public int TargetProductId { get; set; }
        public int? SuggestedProductId { get; set; }
        public string ToId { get; set; }

    }
}
