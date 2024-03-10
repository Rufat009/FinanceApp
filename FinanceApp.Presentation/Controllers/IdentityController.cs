
using System.Net;
using System.Security.Claims;
using FinanceApp.Core.Dtos;
using FinanceApp.Core.Repositories;
using FinanceApp.Infrastructure.Respositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Presentation.Controllers
{
    public class IdentityController : Controller
    {
      
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }


       // [HttpPost]
        // public async Task<IActionResult> Login([FromForm] LoginDto loginDto)
        // {
        //     try
        //     {
        //         var user = await repository.GetUserByEmail(loginDto.Email);

        //         await repository.CheckPassword(loginDto);

        //         HttpContext.Response.Cookies.Append("UserId", user.Email.ToString());

        //         var claims = new List<Claim> {
        //             new("creationDate", DateTime.UtcNow.ToString()),
        //         };

        //         var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        //         await HttpContext.SignInAsync(
        //             scheme: CookieAuthenticationDefaults.AuthenticationScheme,
        //             principal: new ClaimsPrincipal(claimsIdentity)
        //         );

        //         if (string.IsNullOrWhiteSpace(loginDto.ReturnUrl))
        //             return RedirectToAction("Index", "Home");

        //         return RedirectPermanent(loginDto.ReturnUrl);
        //     }
        //     catch (ArgumentException ex)
        //     {
        //         return BadRequest(ex.Message);
        //     }
        //     catch (Exception)
        //     {
        //         return StatusCode(500, "Error!");
        //     }
        // }

        // [HttpPost]
        // public async Task<IActionResult> Registration(UserDto userDto)
        // {
           

        //     await repository.CreateAsync(userDto);

        //     return RedirectToAction("Login", "Identity");
        // }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return base.RedirectToAction("Login", "Identity");
        }
    }
}