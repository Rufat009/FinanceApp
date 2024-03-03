
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
        private readonly IUserRepository repository;

        public IdentityController(IUserRepository repository)
        {
            this.repository = repository;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }


        [HttpPost]

        public async Task<IActionResult> Login([FromForm] LoginDto loginDto)
        {


            var login = await repository.LoginAsync(loginDto);

            if (login is null)
                return BadRequest("Incorrect Login!");


            int? userId = await repository.GetIdByEmail(loginDto.Email);

            HttpContext.Response.Cookies.Append("UserId", userId.ToString());

            var claims = new List<Claim> {
                new("creationDate", DateTime.UtcNow.ToString()),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                scheme: CookieAuthenticationDefaults.AuthenticationScheme,
                principal: new ClaimsPrincipal(claimsIdentity)
            );

            if (string.IsNullOrWhiteSpace(loginDto.ReturnUrl))
                return RedirectToAction("Index", "Home");

            return RedirectPermanent(loginDto.ReturnUrl);



        }
        [HttpPost]
        public async Task<IActionResult> Registration(UserDto userDto)
        {

            await repository.CreateAsync(userDto);

            return RedirectToAction("Login", "Identity");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return base.RedirectToAction("Login", "Identity");
        }



    }
}