using System.Formats.Asn1;
using FinanceApp.Core.Models;
using FinanceApp.Core.Dtos;
using FinanceApp.Core.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Controllers;

public class FinanceController : Controller
{
    private ITransactionRepository transactionRepository;

    public FinanceController(ITransactionRepository transactionRepository)
    {
        this.transactionRepository = transactionRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var transaction = await transactionRepository.GetAllAsync();

        return View(transaction);
    }

    [Authorize]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(TransactionDto transactionDto)
    {
        if (transactionDto.Description == null)
        {
            return BadRequest("None description");
        }

        if (transactionDto.Amount <= 0)
        {
            return BadRequest("Amount can not be less than 0");
        }

        await transactionRepository.CreateAsync(transactionDto);

        return RedirectToAction("GetAll", "Finance");
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




}