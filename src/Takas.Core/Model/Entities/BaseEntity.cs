using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Takas.Core.Model.Interfaces;

namespace Takas.Core.Model.Entities
{
    public class BaseEntity:IBaseEntity
    {
        [Key]
        [Column(Order = 1)]
        public int Id { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
