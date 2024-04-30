
using FinanceApp.Core.Dtos;
using FinanceApp.Core.Models;
using FinanceApp.Core.Repositories;
using FinanceApp.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Infrastructure.Respositories;

public class BillRepository : IBillRepository
{
    private readonly FinanceAppDbContext context;

    public BillRepository(FinanceAppDbContext context)
    {
        this.context = context;
    }

    public async Task<Bill> CreateAsync(Service service, User user, double amountSpent)
    {
        var bill = new Bill
        {
            PayDate = DateTime.Now,
            User = user,
            Service = service,
            AmountSpent = amountSpent
        };

        await context.Bills.AddAsync(bill);

        await context.SaveChangesAsync();

        bill.Id = (await context.Bills.OrderBy(e => e.PayDate).LastOrDefaultAsync()).Id;

        return bill;

    }

    public async Task DeleteAsync(int id)
    {
        context.Bills.Remove(await GetByIdAsync(id));

        await context.SaveChangesAsync();

    }

    public async Task<Bill> GetLatestBillForUser(string userId)
    {
        return await context.Bills
            .Where(b => b.User.Id == userId)
            .OrderByDescending(b => b.PayDate)
            .FirstOrDefaultAsync();
    }
    public async Task<IEnumerable<Bill>> GetAllAsync()
    {
        return await context.Bills.Include("User").Include("Service").ToListAsync();
    }

    public async Task<Bill> GetByIdAsync(int id)
    {
        return await context.Bills.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task UpdateAsync(Bill bill)
    {
        var result = await context.Bills.FirstOrDefaultAsync(x => x.Id == bill.Id);

        result.PayDate = bill.PayDate;

        await context.SaveChangesAsync();

    }
}