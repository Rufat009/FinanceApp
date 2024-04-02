using System.Formats.Asn1;
using FinanceApp.Core.Models;
using FinanceApp.Core.Dtos;
using FinanceApp.Core.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using FinanceApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using FinanceApp.Core.Services;
using System.Security.Claims;
using FluentValidation;
using FinanceApp.Infrastructure.Services;

namespace FinanceApp.Controllers;

[Authorize]
public class FinanceController : Controller
{
    private IBillService billService;
    private IServiceService addserviceService;
    private IServiceRepository serviceRepository;
    private readonly UserManager<User> userManager;

    private readonly IValidator<ServiceDto> serviceValidator;
    private readonly IServiceService serviceService;
    private readonly IUserService userService;
    public FinanceController(IBillService billService, IServiceRepository serviceRepository, UserManager<User> userManager, IServiceService serviceService, IUserService userService, IValidator<ServiceDto> serviceValidator,IServiceService addserviceService)
    {
        this.userManager = userManager;
        this.serviceService = serviceService;
        this.billService = billService;
        this.serviceRepository = serviceRepository;
        this.userService = userService;
        this.serviceValidator = serviceValidator;
        this.addserviceService = addserviceService;
    }

    [HttpGet]
    public async Task<IActionResult> History()
    {
        return View(await billService.History(User));
    }


    public async Task<IActionResult> Delete(int id)
    {
        await billService.DeleteAsync(id);

        return RedirectToAction("History");
    }

    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        try
        {
            var bill = await billService.GetByIdAsync(id);

            return base.View(model: bill);
        }

        catch (NullReferenceException)
        {
            return RedirectToAction("NotFound", "Home");
        }

    }

    [HttpPost]

    public async Task<IActionResult> Update(Bill bill)
    {

        await billService.UpdateAsync(bill);

        return RedirectToAction("History");
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Services()
    {
        return View(await serviceRepository.GetAll());
    }

    [HttpGet]
    public async Task<IActionResult> Payment(int id)
    {
        return View(await serviceService.Payment(User, id));
    }

    [HttpPost]
    public async Task<IActionResult> Bill(int id)
    {
        var service = await serviceRepository.GetById(id);

        var user = await userManager.GetUserAsync(User);

        var bill = await billService.CreateAsync(service, user);

        return View(bill);

    }

    [HttpPut]
    public async Task<IActionResult> Accept(double amount)
    {
        await userService.ChangeBalance(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!, -amount);
        return RedirectToAction("Profile", "Identity");
    }

    [HttpGet]
    public IActionResult AddService(){

        return View();
    }


    [HttpPost]
    public async Task<IActionResult> Search(string service)
    {
        var result = await serviceService.Search(service);

        return View("Services", result);
    }


    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddService(ServiceDto serviceDto)
    {
        try
        {
            var result = serviceValidator.Validate(serviceDto);
            if (result.IsValid == false)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(
                        key: error.PropertyName,
                        errorMessage: error.ErrorMessage
                    );
                }
                return View("AddService");
            }
            await serviceService.Add(serviceDto);
            return RedirectToAction("Services");
        }
        catch (Exception ex)
        {
            return RedirectToAction("Error", "ErrorPage", new { message = ex.Message });
        }
    }
}
