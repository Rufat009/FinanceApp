using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceApp.Core.Models;

namespace FinanceApp.Core.Services
{
    public interface IUserService
    {
        public Task ChangeBalance(string id,double toadd);
        public Task UpdateUser(string id,User user);
    }
}