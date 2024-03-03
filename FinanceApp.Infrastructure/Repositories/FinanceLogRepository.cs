
using Dapper;
using FinanceApp.Core.Models;
using FinanceApp.Core.Repositories;
using Microsoft.Data.SqlClient;

namespace FinanceApp.Infrastructure.Respositories
{
    public class FinanceLogRepository : ILogRepository
    {
        private readonly SqlConnection connection;
        public FinanceLogRepository(SqlConnection connection)
        {
            this.connection = connection;
        }
        public async Task CreateAsync(Log myLog)
        {
            string sql = "insert into Logs (UserId, Url, MethodType, StatusCode, RequestBody, ResponseBody) values (@UserId, @Url, @MethodType, @StatusCode, @RequestBody, @ResponseBody)";

            await connection.ExecuteAsync(sql, myLog);
        }


    }
}