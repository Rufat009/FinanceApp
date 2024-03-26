
using  FinanceApp.Core.Dtos;
using FinanceApp.Core.Models;

namespace FinanceApp.Core.Repositories;

public interface IBillRepository 
{
    Task<IEnumerable<Bill>> GetAllAsync();

    Task<Bill> GetByIdAsync(int id);

    Task<Bill> CreateAsync(Service service, User user);

    Task DeleteAsync(int id);

    Task UpdateAsync(Bill bill);


}