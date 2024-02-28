
using System.Net;
using System.Security.Claims;
using FinanceApp.Core.Dtos;
using FinanceApp.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Presentation.Controllers
{
    public class IdentityController : Controller
    {
        private readonly IUserService service;

        public IdentityController(IUserService service)
        {
            this.service = service;
        }

        
        [HttpGet]
        public IActionResult Login(string? returnUrl)
        {

            ViewData["returnUrl"] = returnUrl;
            return View();
        }

        [HttpGet]
        public IActionResult Registration(/*[FromForm] UserDto userDto*/){
            return View();
        }


        [HttpPost]

        public async Task<IActionResult> Login([FromForm] LoginDto loginDto){

         try
        {
            await service.LoginAsync(loginDto);

            int userId = await service.GetIdByLogin(loginDto.Login);

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
        catch (ArgumentException ex)
        {
            HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            ViewData["ErrorMessage"] = ex.Message;
            return View();
        }
        catch (Exception)
        {
            return StatusCode(500, "Something Went Wrong!");
        }
        }



     [HttpPost]
    public async Task<IActionResult> Registration(UserDto userDto)
    {
        try
        {
            await service.CreateAsync(userDto);
        }
        catch (ArgumentException ex)
        {
            HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            ViewData["ErrorMessage"] = ex.Message;
            return View();
        }
        catch (Exception)
        {
            return StatusCode(500, "Error!");
        }

        return RedirectToAction("Login", "Identity");
    }
    }
}