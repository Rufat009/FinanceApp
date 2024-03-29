

using Dapper;
using FinanceApp.Core.Dtos;
using FinanceApp.Core.Models;
using FinanceApp.Core.Repositories;
using Microsoft.Data.SqlClient;

namespace FinanceApp.Infrastructure.Respositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly SqlConnection connection;
    public TransactionRepository(SqlConnection connection)
    {
        this.connection = connection;
    }


    public async Task<IEnumerable<Transaction>> GetAllAsync()
	{
		string sql = "select * from Transactions";
		var transaction = await connection.QueryAsync<Transaction>(sql);

		return transaction;
	}
	public async Task<Transaction> GetByIdAsync(int id)
	{

		string sql = "select * from Transactions where Id = @Id";
		var transaction = await connection.QueryFirstOrDefaultAsync<Transaction>(sql, new { Id = id });

		return transaction;
	}

	public async Task CreateAsync(TransactionDto transaction)
	{

		string sql = @"insert into Transactions ([Amount], [Description])
                         values( @Amount, @Description)";

		await connection.ExecuteAsync(sql, transaction);
	}

    
}