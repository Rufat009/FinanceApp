using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceApp.Models;

namespace FinanceApp.Repositories.Base
{
    public interface ILogRepository
    {
        public  Task CreateAsync(Log myLog);
    }
}