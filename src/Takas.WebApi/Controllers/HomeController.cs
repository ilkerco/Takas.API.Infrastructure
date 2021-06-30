using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Takas.WebApi.Controllers
{
    public class HomeController:Controller
    {
        [HttpGet("isAlive")]
        public IActionResult IsAlive()
        {
            return Ok();
        }
    }
}
