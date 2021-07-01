using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Takas.WebApi.Dto
{
    public class UpdateUserRequest
    {
        public string DisplayName { get; set; }
        public string PhotoUrl { get; set; }
    }
}
