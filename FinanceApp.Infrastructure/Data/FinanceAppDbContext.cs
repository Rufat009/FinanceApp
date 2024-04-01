using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceApp.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Infrastructure.Data
{
    public class FinanceAppDbContext : IdentityDbContext<User, IdentityRole, string>
    {

        public DbSet<Log> Logs { get; set; }
        public DbSet<Bill> Bills {get; set;}
        public DbSet<Service> Services {get; set;}


          public FinanceAppDbContext(DbContextOptions<FinanceAppDbContext> options) : base(options) {}
    }
}