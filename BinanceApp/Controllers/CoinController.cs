using BinanceApp.Attributes;
using BinanceApp.Controllers.Base;
using BinanceApp.Extensions;
using BinanceApp.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BinanceApp.Controllers;

public class CoinController : ControllerBase
{

    private const string ConnectionString = "Server=localhost;Database=BinanceAppDb;Trusted_Connection=True;TrustServerCertificate=True;";

    [HttpGet("GetAll")]
    public async Task GetCoinsAsync()
    {
        using var writer = new StreamWriter(base.HttpContext.Response.OutputStream);

        using var connection = new SqlConnection(ConnectionString);
        var coins = await connection.QueryAsync<Cryptocurrency>("select * from Cryptocurrencies");

        var coinsHtml = coins.GetHtml();
        await writer.WriteLineAsync(coinsHtml);
        base.HttpContext.Response.ContentType = "text/html";

        base.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
    }

    [HttpGet("GetById")]
    public async Task GetCoinsByIdAsync()
    {
        var coinIdToGetObj = base.HttpContext.Request.QueryString["id"];

        if (coinIdToGetObj == null || int.TryParse(coinIdToGetObj, out int coinIdToGet) == false)
        {
            base.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }

        using var connection = new SqlConnection(ConnectionString);
        var coin = await connection.QueryFirstOrDefaultAsync<Cryptocurrency>(
            sql: "select top 1 * from Cryptocurrencies where Id = @Id",
            param: new { Id = coinIdToGet });

        if (coin is null)
        {
            base.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
            return;
        }

        using var writer = new StreamWriter(base.HttpContext.Response.OutputStream);
        await writer.WriteLineAsync(JsonSerializer.Serialize(coin));

        base.HttpContext.Response.ContentType = "application/json";
        base.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
    }



    [HttpPost("Create")]
    public async Task PostCoinsAsync()
    {
        using var reader = new StreamReader(base.HttpContext.Request.InputStream);
        var json = await reader.ReadToEndAsync();

        var newCoin = JsonSerializer.Deserialize<Cryptocurrency>(json);

        if (newCoin == null || newCoin.Price == null || string.IsNullOrWhiteSpace(newCoin.Name) || newCoin.Count == null)
        {
            base.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }

        using var connection = new SqlConnection(ConnectionString);
        var coins = await connection.ExecuteAsync(
            @"insert into Cryptocurrencies (Name, Price, Count) 
        values(@Name, @Price,@Count)",
            param: newCoin
           );

        base.HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;
    }

    [HttpDelete]
    public async Task DeleteCoinsAsync()
    {
        var coinIdToDeleteObj = base.HttpContext.Request.QueryString["id"];

        if (coinIdToDeleteObj == null || int.TryParse(coinIdToDeleteObj, out int coinIdToDelete) == false)
        {
            base.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }

        using var connection = new SqlConnection(ConnectionString);
        var deletedRowsCount = await connection.ExecuteAsync(
            @"delete Cryptocurrencies
        where Id = @Id",
            param: new
            {
                Id = coinIdToDeleteObj,
            });

        if (deletedRowsCount == 0)
        {
            base.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
            return;
        }

        base.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
    }

    [HttpPut]
    public async Task PutCoinAsync()
    {
        var coinIdToUpdateObj = base.HttpContext.Request.QueryString["id"];

        if (coinIdToUpdateObj == null || int.TryParse(coinIdToUpdateObj, out int coinIdToUpdate) == false)
        {
            base.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }

        using var reader = new StreamReader(base.HttpContext.Request.InputStream);
        var json = await reader.ReadToEndAsync();

        var myCoinToUpdate = JsonSerializer.Deserialize<Cryptocurrency>(json);

        if (myCoinToUpdate == null || myCoinToUpdate.Price == null || string.IsNullOrEmpty(myCoinToUpdate.Name) || myCoinToUpdate.Count == null)
        {
            base.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }

        using var connection = new SqlConnection(ConnectionString);
        var affectedRowsCount = await connection.ExecuteAsync(
            @"update Cryptocurrencies
        set Name = @Name, Price = @Price, Count = @Count
        where Id = @Id",
            param: new
            {
                myCoinToUpdate.Name,
                myCoinToUpdate.Price,
                myCoinToUpdate.Count,

                Id = coinIdToUpdate
            });

        if (affectedRowsCount == 0)
        {
            base.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
            return;
        }

        base.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
    }
}
