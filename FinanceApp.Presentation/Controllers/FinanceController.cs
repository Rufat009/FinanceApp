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
    private ITransactionRepository transactionRepository;
    private IServiceRepository serviceRepository;

    private readonly UserManager<User> userManager;
    private readonly FinanceAppDbContext myDbContext;

    public FinanceController(ITransactionRepository transactionRepository, IServiceRepository serviceRepository, UserManager<User> userManager, FinanceAppDbContext myDbContext)
    {
        this.userManager = userManager;
        this.myDbContext = myDbContext;
        this.transactionRepository = transactionRepository;
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
            return View(await myDbContext.Bills.Include("User").Include("Service").Where(p => p.User.Id == user.Id).ToListAsync());
        }



        return View(await myDbContext.Bills.Include("User").Include("Service").ToListAsync());
    }


    public async Task<IActionResult> Delete(int id)
    {
        myDbContext.Bills.Remove(await myDbContext.Bills.FirstOrDefaultAsync(p => p.Id == id));

        await myDbContext.SaveChangesAsync();

        return RedirectToAction("History");
    }

    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
       var bill = await myDbContext.Bills.FirstOrDefaultAsync(p => p.Id == id);

        return base.View(model: bill);
    }

    [HttpPost]

    public async Task<IActionResult> Update(Bill bill)
    {

        var result = await myDbContext.Bills.FirstOrDefaultAsync(x => x.Id == bill.Id);

		result.PayDate = bill.PayDate;

		await myDbContext.SaveChangesAsync();

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

        var bill = new Bill
        {
            PayDate = DateTime.Now,
            User = user,
            Service = service
        };

        await myDbContext.Bills.AddAsync(bill);

        await myDbContext.SaveChangesAsync();

        bill.Id = (await myDbContext.Bills.OrderBy(e => e.PayDate).LastOrDefaultAsync()).Id;

        return View(bill);


    }

}