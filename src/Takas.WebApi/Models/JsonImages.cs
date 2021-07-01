using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Takas.WebApi.Models
{
    public class JsonImages
    {
        [JsonProperty]
        public List<string> images { get; set; }
    }
}
