using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Takas.WebApi.Dto
{
    public class ProductResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string OwnerId { get; set; }
        public int Price { get; set; }
        public string Category { get; set; }
        public IEnumerable<string> Images { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
