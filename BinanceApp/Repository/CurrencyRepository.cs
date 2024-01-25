using BinanceApp.Dtos;
using BinanceApp.Models;

namespace BinanceApp.Repository;

public class CurrencyRepository
{
    private readonly string? connectionString;

    public CurrencyRepository(IConfiguration configuration)
    {
        connectionString = configuration.GetConnectionString("BinanceAppDb");
    }

    public async Task<IEnumerable<Currency>> GetAll()
    {
        using var connection = new SqlConnection(connectionString);

        string sql = "select * from Currencies";
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
