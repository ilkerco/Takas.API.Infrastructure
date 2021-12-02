using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Takas.Core.Model.Entities
{
    public class Chat
    {

        public Chat()
        {
            Messages = new List<Message>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? SuggestedProductId { get; set; }
        public string ChatName { get; set; }
        public virtual Product SuggestedProduct { get; set; }
        public int TargetProductId { get; set; }
        public virtual Product TargetProduct { get; set; }
        public ICollection<Message> Messages { get; set; }
        public string ToId { get; set; }
        public User To { get; set; }
        public string FromId { get; set; }
        public User From { get; set; }
        //prop for users
    }

    

}
