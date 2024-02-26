using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceApp.Core.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Presentation.Controllers
{
    public class IdentityController : Controller
    {
        [HttpGet]
        public IActionResult Login(string? returnUrl)
        {

            ViewData["returnUrl"] = returnUrl;
            return View();
        }

        [HttpGet]

        public IActionResult Registration([FromForm] UserDto userDto){
            return View();
        }

        [HttpPost]

        public IActionResult Login( )
    }
}