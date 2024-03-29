using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FinanceApp.Core.Models;

namespace FinanceApp.Core.Services
{
    public interface IBillService
    {
        public Task<Bill> CreateAsync(Service service, User user);
        public Task<Bill> GetByIdAsync(int id);
        public Task UpdateAsync(Bill bill);
        public Task DeleteAsync(int id);
         public  Task<IEnumerable<Bill>> History( ClaimsPrincipal User);





    }
}