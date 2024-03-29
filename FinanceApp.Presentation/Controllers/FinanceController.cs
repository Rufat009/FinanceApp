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

namespace FinanceApp.Controllers;

[Authorize]
public class FinanceController : Controller
{
    private IBillService billService;
    private IServiceRepository serviceRepository;

    private readonly UserManager<User> userManager;
    private readonly IServiceService serviceService;

    public FinanceController(IBillService billService, IServiceRepository serviceRepository, UserManager<User> userManager, IServiceService serviceService)
    {
        this.userManager = userManager;
        this.serviceService = serviceService;
        this.billService = billService;
        this.serviceRepository = serviceRepository;
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


}