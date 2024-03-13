using Dapper;
using FinanceApp.Core.Dtos;
using FinanceApp.Core.Models;
using FinanceApp.Core.Repositories;
using Microsoft.Data.SqlClient;

namespace FinanceApp.Infrastructure.Respositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SqlConnection connection;
        public UserRepository(SqlConnection connection)
        {
            this.connection = connection;
        }
        public async Task CreateAsync(UserDto userDto)
        {
            string query = @"insert into Users([Name], [Email], [Password], [Age], [Surname], [Balance])
                        values(@Name, @Email, @Password, @Age, @Surname, @Balance)";

            await connection.ExecuteAsync(query, userDto);
        }

        public async Task<UserDto> GetUserByEmail(string email)
        {
            string query = "select * from Users where Email = @Email";

            var user = await connection.QueryFirstOrDefaultAsync<UserDto>(query, new { Email = email });

            if (user is null)
            {
                throw new ArgumentException($"There is no email: {email}");
            }

            return user;
        }

        public async Task CheckPassword(LoginDto login)
        {
            string query = "select * from Users where Password = @Password";

            var user = await connection.QueryFirstOrDefaultAsync<UserDto>(query, login);

            if (user is null || login.Email != user.Email)
            {
                throw new ArgumentException("Incorrect password!");
            }
        }
    }
}