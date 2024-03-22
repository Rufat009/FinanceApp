using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceApp.Core.Models;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceApp.Core.Repositories
{
    public interface IServiceRepository 
    {
        public Task<IEnumerable<Service>> GetAll();
    }
}