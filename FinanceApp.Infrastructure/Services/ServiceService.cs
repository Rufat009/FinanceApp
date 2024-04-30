using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FinanceApp.Core.Dtos;
using FinanceApp.Core.Models;
using FinanceApp.Core.Repositories;
using FinanceApp.Core.Services;
using FinanceApp.Core.ViewModels;
using FinanceApp.Infrastructure.Data;
using FinanceApp.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Infrastructure.Services
{
    public class ServiceService : IServiceService
    {
        private readonly IServiceRepository serviceRepository;
        private readonly UserManager<User> userManager;
        private readonly FinanceAppDbContext dbContext;

        public ServiceService(IServiceRepository serviceRepository, UserManager<User> userManager, FinanceAppDbContext dbContext)
        {
            this.serviceRepository = serviceRepository;
            this.userManager = userManager;
            this.dbContext = dbContext;
        }


        public async Task Add(ServiceDto serviceDto)
        {
            var fileExtension = new FileInfo(serviceDto.ServiceImageUrl.FileName).Extension;

            var filename = $"{serviceDto.ServiceName}{fileExtension}";

            var destinationServiceImagePath = $"wwwroot/Assets/{filename}";

            using var fileStream = System.IO.File.Create(destinationServiceImagePath);
            await serviceDto.ServiceImageUrl.CopyToAsync(fileStream);

            var service = new Service()
            {
                ImageUrl = $"Assets/{filename}",
                Name = serviceDto.ServiceName,
                ServiceCost = serviceDto.ServiceCost,

            };
            await dbContext.Services.AddAsync(service);
            await dbContext.SaveChangesAsync();
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

        public async Task<IEnumerable<Service>> Search(string service)
        {
            if (string.IsNullOrEmpty(service))
            {
                return await dbContext.Services.ToArrayAsync();
            }

            var result = await dbContext.Services.Where(x => x.Name.Contains(service)).ToArrayAsync();
            return result;
        }

        public async Task DeleteServiceAsync(int id)
        {
            var service = await this.dbContext.Services.FirstOrDefaultAsync(service => service.Id == id);

            this.dbContext.Remove<Service>(service!);

            await this.dbContext.SaveChangesAsync();
        }
    }
}