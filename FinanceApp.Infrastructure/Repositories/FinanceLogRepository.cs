using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using FinanceApp.Models;
using FinanceApp.Repositories.Base;
using Microsoft.Data.SqlClient;

namespace FinanceApp.Repositories
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