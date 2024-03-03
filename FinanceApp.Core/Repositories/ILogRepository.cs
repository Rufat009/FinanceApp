using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceApp.Core.Models;

namespace FinanceApp.Core.Repositories
{
    public interface ILogRepository
    {
        public  Task CreateAsync(Log myLog);
    }
}