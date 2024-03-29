
using System.Net;
using System.Security.Claims;
using FinanceApp.Core.Dtos;
using FinanceApp.Core.Models;
using FinanceApp.Core.Repositories;
using FinanceApp.Infrastructure.Respositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Presentation.Controllers;

[Authorize]
public class IdentityController : Controller
{
    private readonly UserManager<User> userManager;
    private readonly RoleManager<IdentityRole> roleManager;
    private readonly SignInManager<User> signInManager;

    public IdentityController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager)
    {
        this.userManager = userManager;
        this.roleManager = roleManager;
        this.signInManager = signInManager;
    }

    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();

        return RedirectToAction("Index", "Home");
    }

    [AllowAnonymous]
    public IActionResult Registration()
    {
        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Registration(UserDto userDto)
    {
        if (ModelState.IsValid == false)
        {
            return View();
        }

        var user = new User
        {
            UserName = userDto.Name,
            Surname = userDto.Surname,
            Age = userDto.Age,
            Email = userDto.Email,

        };

        var result = await userManager.CreateAsync(user, userDto.Password);

        if (!result.Succeeded)
        {

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            if (ModelState.Any())
            {
                return View();
            }

        }

        var userRole = new IdentityRole
        {
            Name = "User"
        };

        await roleManager.CreateAsync(userRole);
        await userManager.AddToRoleAsync(user, "User");

        return RedirectToAction("Login");
    }

    [AllowAnonymous]
    public async Task<IActionResult> CreateAdmin()
    {
        var user = new User
        {
            UserName = "Admin",
            Email = "admin@gmail.com"
        };

        var result = await userManager.CreateAsync(user, "Admin123!");

        var userRole = new IdentityRole
        {
            Name = "Admin"
        };

        await roleManager.CreateAsync(userRole);
        await userManager.AddToRoleAsync(user, "Admin");

        return RedirectToAction("Login");
    }

    [AllowAnonymous]
    public IActionResult Login(string? ReturnUrl)
    {
        ViewData["ReturnUrl"] = ReturnUrl;

        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Login(LoginDto userdto)
    {
        if (ModelState.IsValid == false)
        {
            return View();
        }

        var user = await userManager.FindByEmailAsync(userdto.Email);

        if (user is null)
        {
            ModelState.AddModelError("Incorrect Email", "No user with this email found");
            return View();
        }

        var result = await signInManager.PasswordSignInAsync(user, userdto.Password, true, true);

        if (result.Succeeded == false)
        {
            ModelState.AddModelError("Incorrect Password", "Incorrect Credentials");

            return View();
        }

        return RedirectPermanent(userdto.ReturnUrl ?? "/");

    }
    public async Task<IActionResult> Profile()
    {
        var user = await userManager.GetUserAsync(User);

        return View(user);
    }


    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        var result = await userManager.FindByIdAsync(id);

        return View(result);

    }

    [HttpPost]
    public async Task<IActionResult> Edit(User user)
    {
        var oldUser = await userManager.GetUserAsync(base.HttpContext.User);

        oldUser.UserName = user.UserName;

        oldUser.Email = user.Email;
        
        oldUser.Surname = user.Surname;
        
        oldUser.Age = user.Age;

        await userManager.UpdateAsync(oldUser);
        
        return RedirectToAction("Profile");

    }

    [AllowAnonymous]
    public IActionResult AccessDenied(){
        return View();
    }
}

