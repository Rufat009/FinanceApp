
using  FinanceApp.Core.Dtos;
using FinanceApp.Core.Models;

namespace FinanceApp.Core.Repositories;

public interface ITransactionRepository 
{
    public  Task<IEnumerable<Transaction>> GetAllAsync();

    public  Task<Transaction> GetByIdAsync(int id);

    public Task CreateAsync(TransactionDto transaction);
}
