using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Takas.Core.Model.Entities
{
    public class ProductImage:BaseEntity
    {
        public string ImageSource { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
