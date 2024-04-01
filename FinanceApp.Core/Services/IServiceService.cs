using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FinanceApp.Core.Models;
using FinanceApp.Core.ViewModels;

namespace FinanceApp.Core.Services
{
    public interface IServiceService
    {
        public  Task<IEnumerable<Service>> GetAll();

        public  Task<Service> GetById(int id);
        public  Task<PaymentViewModel> Payment(ClaimsPrincipal User, int id);


        
    }
}