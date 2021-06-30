using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace Takas.Core.Model.Entities
{
    public class User:IdentityUser
    {
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
