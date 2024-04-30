
using System.Net;
using System.Security.Claims;
using FinanceApp.Core.Dtos;
using FinanceApp.Core.Models;
using FinanceApp.Core.Repositories;
using FinanceApp.Core.Services;
using FinanceApp.Infrastructure.Data;
using FinanceApp.Infrastructure.Respositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Presentation.Controllers;

[Authorize]
public class IdentityController : Controller
{
    private readonly UserManager<User> userManager;
    private readonly RoleManager<IdentityRole> roleManager;
    private readonly SignInManager<User> signInManager;
    private readonly IUserService userService;
    private readonly FinanceAppDbContext dbContext;

    public IdentityController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager, IUserService userService, FinanceAppDbContext dbContext)
    {
        this.userManager = userManager;
        this.roleManager = roleManager;
        this.signInManager = signInManager;
        this.userService = userService;
        this.dbContext = dbContext;
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
            AbonentNumber = userDto.AbonentNumber,
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


    public async Task<IActionResult> ProfileByAbonentNumber(int AbonentNumber)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(x => x.AbonentNumber == AbonentNumber);

        return View("Profile", user);
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
        await userService.UpdateUser(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!, user);

        return RedirectToAction("Profile");

    }


    [HttpGet]
    public async Task<IActionResult> ChangeBalance()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ChangeBalance([FromForm] double balance)
    {
        await userService.ChangeBalance(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!, balance);
        return RedirectToAction("Profile");
    }




    [AllowAnonymous]
    public IActionResult AccessDenied()
    {
        return View();
    }


    // [HttpPut]
    // [Authorize]
    // public async Task<IActionResult> ChangeAvatar(IFormFile avatar)
    // {
    //     var userName = User.Identity.Name;
    //     var fileName = $"{userName}{Path.GetExtension(avatar.FileName)}";

    //     var destinationFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
    //     var destinationPath = Path.Combine(destinationFolder, fileName); 

    //     if (System.IO.File.Exists(destinationPath))
    //     {
    //         System.IO.File.Delete(destinationPath);
    //     }

    //     Directory.CreateDirectory(destinationFolder);

    //     using (var stream = new FileStream(destinationPath, FileMode.Create))
    //     {
    //         await avatar.CopyToAsync(stream);
    //     }

    //     var relativePath = "/uploads/" + fileName;

    //     await userService.UpdateAvatar(relativePath, User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

    //     return RedirectToAction("Profile");
    // }
}

