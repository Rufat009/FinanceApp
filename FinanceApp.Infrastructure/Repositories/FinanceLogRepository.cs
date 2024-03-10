

using FinanceApp.Core.Models;
using FinanceApp.Core.Repositories;
using FinanceApp.Infrastructure.Data;
using Microsoft.Data.SqlClient;

namespace FinanceApp.Infrastructure.Respositories
{
    public class FinanceLogRepository : ILogRepository
    {
        private readonly FinanceAppDbContext context;

        public FinanceLogRepository(FinanceAppDbContext context)
        {
            this.context = context;
        }
        public async Task CreateAsync(Log myLog)
        {
            await context.Logs.AddAsync(myLog);
            await context.SaveChangesAsync();

        }


    }
}