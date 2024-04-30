using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceApp.Core.Models;
using FinanceApp.Core.Services;
using FinanceApp.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly FinanceAppDbContext dbContext;
        private readonly UserManager<User> userManager;
        public UserService(FinanceAppDbContext dbContext, UserManager<User> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        public async Task ChangeBalance(string id, double toadd)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new Exception("Error: User not found.");
            }
            if (toadd < 0 && user.Balance + toadd < 0)
            {
                throw new Exception("Error: Insufficient funds.");
            }
            user.Balance += toadd;
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateUser(string id, User user)
        {
            var oldUser = await userManager.FindByIdAsync(id);

            oldUser.UserName = user.UserName;

            oldUser.Email = user.Email;

            oldUser.Surname = user.Surname;

            oldUser.Age = user.Age;

            oldUser.AbonentNumber = user.AbonentNumber;

            await dbContext.SaveChangesAsync();
        }

        public async Task<User> Search(string abonentNumber)
        {
            var result = await dbContext.Users.FirstOrDefaultAsync(x => x.AbonentNumber.Equals(abonentNumber));
            return result;
        }
    }

}