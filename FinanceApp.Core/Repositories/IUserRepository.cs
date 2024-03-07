using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceApp.Core.Dtos;

namespace FinanceApp.Core.Repositories
{
    public interface IUserRepository
    {
        Task CreateAsync(UserDto userDto);
        Task<UserDto> GetUserByEmail(string email);
        Task CheckPassword(LoginDto login);
    }
}