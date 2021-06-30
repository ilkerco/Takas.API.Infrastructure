using System;
using System.Collections.Generic;
using System.Text;

namespace Takas.Core.Model.Entities
{
    public class Product:BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string OwnerId { get; set; }
    }
}
