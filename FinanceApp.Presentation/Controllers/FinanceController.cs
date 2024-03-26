using System.Formats.Asn1;
using FinanceApp.Core.Models;
using FinanceApp.Core.Dtos;
using FinanceApp.Core.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using FinanceApp.Presentation.ViewModels;
using FinanceApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Controllers;

public class FinanceController : Controller
{
    private IBillRepository billRepository;
    private IServiceRepository serviceRepository;

    private readonly UserManager<User> userManager;

    public FinanceController(IBillRepository billRepository, IServiceRepository serviceRepository, UserManager<User> userManager)
    {
        this.userManager = userManager;
        this.billRepository = billRepository;
        this.serviceRepository = serviceRepository;
    }

    [HttpGet]
    public async Task<IActionResult> History()
    {
        var user = await userManager.GetUserAsync(User);

        var roles = await userManager.GetRolesAsync(user);

        var result = roles.FirstOrDefault(p => p == "Admin");

        if (result is null)
        {
            return View((await billRepository.GetAllAsync()).Where(p => p.User.Id == user.Id));
        }

        return View(await billRepository.GetAllAsync());
    }


    public async Task<IActionResult> Delete(int id)
    {
        await billRepository.DeleteAsync(id);

        return RedirectToAction("History");
    }

    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var bill = await billRepository.GetByIdAsync(id);

        return base.View(model: bill);
    }

    [HttpPost]

    public async Task<IActionResult> Update(Bill bill)
    {

        await billRepository.UpdateAsync(bill);

        return RedirectToAction("History");
    }

    [HttpGet]
    public async Task<IActionResult> Services()
    {
        return View(await serviceRepository.GetAll());
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Payment(int id)
    {
        var service = await serviceRepository.GetById(id);

        var user = await userManager.GetUserAsync(User);

        return View(new PaymentViewModel
        {
            Service = service,
            User = user
        });
    }

    public async Task<IActionResult> Bill(int id)
    {
        var service = await serviceRepository.GetById(id);

        var user = await userManager.GetUserAsync(User);

        var bill = await billRepository.CreateAsync(service, user);

        return View(bill);

    }

}