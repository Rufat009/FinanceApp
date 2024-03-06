using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceApp.Core.Dtos;

namespace FinanceApp.Core.Repositories
{
    public interface IUserRepository
    {
        Task<int?> LoginAsync(LoginDto loginDto);
        Task<int?> GetIdByEmail(string email);
        Task CreateAsync(UserDto userDto);
    }
}