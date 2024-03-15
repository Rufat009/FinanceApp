

using FinanceApp.Core.Dtos;
using FinanceApp.Core.Models;
using FinanceApp.Core.Repositories;
using FinanceApp.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Infrastructure.Respositories;

public class TransactionRepository : ITransactionRepository
{
	private readonly FinanceAppDbContext context;

	public TransactionRepository(FinanceAppDbContext context)
	{
		this.context = context;
	}

	public async Task<IEnumerable<Transaction>> GetAllAsync()
	{
		return await context.Transactions.ToListAsync();
	}
	public async Task<Transaction> GetByIdAsync(int id)
	{
		return await context.Transactions.FirstAsync(o => o.Id == id);
	}

	public async Task CreateAsync(TransactionDto transaction)
	{
		await context.Transactions.AddAsync(new Transaction
		{
			Amount = transaction.Amount,
			Description = transaction.Description
		});
		await context.SaveChangesAsync();
	}

    public async Task DeleteAsync(Transaction transaction)
    {
         context.Transactions.Remove(transaction);
		 await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Transaction transaction)
    {
        context.Transactions.Update(transaction);
		await context.SaveChangesAsync();
    }
}