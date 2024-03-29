using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FinanceApp.Core.Models;
using FinanceApp.Core.Repositories;
using FinanceApp.Core.Services;
using FinanceApp.Core.ViewModels;
using FinanceApp.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Infrastructure.Services
{
    public class ServiceService : IServiceService
    {
        private readonly IServiceRepository serviceRepository;
        private readonly UserManager<User> userManager;

        public ServiceService(IServiceRepository serviceRepository, UserManager<User> userManager)
        {
            this.serviceRepository = serviceRepository;
            this.userManager = userManager;
        }
        public async Task<IEnumerable<Service>> GetAll()
        {
            return await serviceRepository.GetAll();
        }

        public async Task<Service> GetById(int id)
        {
            return await serviceRepository.GetById(id);
        }

        public async Task<PaymentViewModel> Payment(ClaimsPrincipal User, int id)
        {
            var service = await serviceRepository.GetById(id);

            var user = await userManager.GetUserAsync(User);

            return new PaymentViewModel
            {
                Service = service,
                User = user
            };
        }
    }
}