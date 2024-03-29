using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FinanceApp.Core.Models;
using FinanceApp.Core.Repositories;
using FinanceApp.Core.Services;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Infrastructure.Services
{
    public class BillService : IBillService
    {
        private readonly IBillRepository billRepository;
        private readonly UserManager<User> userManager;

        public BillService(IBillRepository billRepository, UserManager<User> userManager)
        {
            this.billRepository = billRepository;
            this.userManager = userManager;
        }
        public async Task DeleteAsync(int id)
        {

            await billRepository.DeleteAsync(id);

        }
        public async Task UpdateAsync(Bill bill)
        {

            await billRepository.UpdateAsync(bill);

        }
        public async Task<Bill> GetByIdAsync(int id)
        {

            var bill = await billRepository.GetByIdAsync(id);
            if(bill is null) {

                throw new NullReferenceException();
            }
            return bill;

        }

        public async Task<Bill> CreateAsync(Service service, User user)
        {

            var bill = await billRepository.CreateAsync(service, user);

            return bill;

        }

        public async Task<IEnumerable<Bill>> History( ClaimsPrincipal User)
        {
            var user = await userManager.GetUserAsync(User);

            var roles = await userManager.GetRolesAsync(user);

            var result = roles.FirstOrDefault(p => p == "Admin");

            if (result is null)
            {
                return (await billRepository.GetAllAsync()).Where(p => p.User.Id == user.Id);
            }

            return await billRepository.GetAllAsync();
        }
    }

}