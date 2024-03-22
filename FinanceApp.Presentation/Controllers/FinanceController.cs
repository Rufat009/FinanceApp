using System.Formats.Asn1;
using FinanceApp.Core.Models;
using FinanceApp.Core.Dtos;
using FinanceApp.Core.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace FinanceApp.Controllers;

public class FinanceController : Controller
{
    private ITransactionRepository transactionRepository;
    private IServiceRepository serviceRepository;


    public FinanceController(ITransactionRepository transactionRepository, IServiceRepository serviceRepository)
    {
        this.transactionRepository = transactionRepository;
        this.serviceRepository = serviceRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var transaction = await transactionRepository.GetAllAsync();

        return View(transaction);
    }


    public async Task<IActionResult> Delete(int id)
    {
        await this.transactionRepository.DeleteAsync(await transactionRepository.GetByIdAsync(id));

        return RedirectToAction("GetAll");
    }

    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var mytransaction = await this.transactionRepository.GetByIdAsync(id);

        return base.View(model: mytransaction);
    }

    [HttpPost]

    public async Task<IActionResult> Update(Transaction transaction)
    {

        await this.transactionRepository.UpdateAsync(transaction);

        return RedirectToAction("GetAll");
    }

    [HttpGet]
    public async Task<IActionResult> Services()
    {
        return View(await serviceRepository.GetAll());
    }

    [HttpGet]
    [Authorize] 
    public IActionResult Payment(int id)
    {
        return View(id);
    }



}