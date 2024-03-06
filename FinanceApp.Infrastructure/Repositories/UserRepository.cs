using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            Console.WriteLine("CreateAsync Start");
            string query = @"insert into Users([Name], [Email], [Password], [Age], [Surname], [Balance])
                        values(@Name, @Email, @Password, @Age, @Surname, @Balance)";

            await connection.ExecuteAsync(query, userDto);
        }

        public async Task<int?> GetIdByEmail(string email)
        {
            string query = "select * from Users where [Email] = @email";

            return (await connection.QueryFirstOrDefaultAsync<User>(query, new {email}))?.Id;

        }

        public async Task<int?> LoginAsync(LoginDto loginDto)
        {

            string userQuery = "select * from Users where [Email] = @Email and [Password] = @Password";

            var user = await connection.QueryFirstOrDefaultAsync<User>(userQuery, loginDto);

            return user?.Id;
        }
    }
}