
using  FinanceApp.Core.Dtos;
using FinanceApp.Core.Models;

namespace FinanceApp.Core.Repositories;

public interface ITransactionRepository 
{
    Task<IEnumerable<Transaction>> GetAllAsync();

    Task<Transaction> GetByIdAsync(int id);

    Task CreateAsync(TransactionDto transaction);

    Task DeleteAsync(Transaction transaction);

    Task UpdateAsync(Transaction transaction);


}