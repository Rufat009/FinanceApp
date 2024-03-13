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
    public IActionResult Create(TransactionDto transactionDto)
    {
        if (transactionDto.Description == null)
        {
            return BadRequest("None description");
        }

        if (transactionDto.Amount <= 0)
        {
            return BadRequest("Amount can not be less than 0");
        }

        transactionRepository.CreateAsync(transactionDto);

        return RedirectToAction("GetAll", "Finance");
    }
}