using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceApp.Core.Models;
using FinanceApp.Core.Repositories;
using FinanceApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Infrastructure.Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly FinanceAppDbContext context;

        public ServiceRepository(FinanceAppDbContext context)
        {
            this.context = context;
            
        }
        public async Task<IEnumerable<Service>> GetAll()
        {
            return await context.Services.ToListAsync();
        }
    }
}