using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceApp.Core.Dtos;

namespace FinanceApp.Infrastructure.Services
{
    public interface IUserService
    {
    Task<int> LoginAsync(LoginDto loginDto);
    Task<int> GetIdByLogin(string login);
    Task CreateAsync(UserDto userDto);

    }
}