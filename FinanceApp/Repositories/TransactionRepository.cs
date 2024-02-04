namespace FinanceApp.Repositories;
using FinanceApp.Models;

public class FinanceRepository
{
	private readonly string? connectionString;

	public FinanceRepository(IConfiguration configuration)
	{
		connectionString = configuration.GetConnectionString("FinanceAppDb");
	}

	public async Task<IEnumerable<Finance>> GetAll()
	{
		using var connection = new SqlConnection(connectionString);

		string sql = "select * from Transactions";
		var currency = await connection.QueryAsync<Currency>(sql);

		return currency;
	}
	public async Task<Currency> GetById(int id)
	{
		using var connection = new SqlConnection(connectionString);

		string sql = "select * from Currenies where Id = @Id";
		var currency = await connection.QueryFirstOrDefaultAsync<Currency>(sql, new { Id = id });

		return currency;
	}

	public async Task Create(CurrencyDto currencyDto)
	{
		using var connection = new SqlConnection(connectionString);

		string sql = @"insert into Currencies ([Name], [Count], [Price])
                         values(@Name, @Count, @Price)";

		await connection.ExecuteAsync(sql, currencyDto);
	}
}