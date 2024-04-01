
using  FinanceApp.Core.Dtos;
using FinanceApp.Core.Models;

namespace FinanceApp.Core.Repositories;

public interface ITransactionRepository 
{
    Task<IEnumerable<Transaction>> GetAllAsync();

    Task<Transaction> GetByIdAsync(int id);

    Task CreateAsync(TransactionDto transaction);
}