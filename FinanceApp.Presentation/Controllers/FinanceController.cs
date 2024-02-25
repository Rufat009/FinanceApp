using FinanceApp.Dtos;
using FinanceApp.Repositories;
using FinanceApp.Repositories.Base;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Controllers;

public class FinanceController : Controller
{
    private ITransactionRepository transactionRepository;

    public FinanceController(ITransactionRepository transactionRepository)
    {
        this.transactionRepository = transactionRepository;
    }

    public async Task<IActionResult> GetAll()
    {
        var transaction = await transactionRepository.GetAllAsync();
        return View(transaction);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(TransactionDto transactionDto)
    {
        if (transactionDto.Description == null || transactionDto.Amount <= 0)
        {
            return BadRequest();
        }

        transactionRepository.CreateAsync(transactionDto);


        return View();
    }

    public IActionResult Index()
    {
        return View();
    }
}
