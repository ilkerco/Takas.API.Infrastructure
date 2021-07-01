using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Takas.WebApi.Dto
{
    public class UserResponse
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public int Coin { get; set; }
        public int Boost { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string SubAdminArea { get; set; }
        public string Country { get; set; }
        public string DisplayName { get; set; }
        public string PhotoUrl { get; set; }
    }
}
