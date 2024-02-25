

using FinanceApp.Core.Dtos;
using FinanceApp.Models;

namespace FinanceApp.Repositories.Base;

public interface ITransactionRepository 
{
    public  Task<IEnumerable<Transaction>> GetAllAsync();

    public  Task<Transaction> GetByIdAsync(int id);

    public Task CreateAsync(TransactionDto transaction);
}
