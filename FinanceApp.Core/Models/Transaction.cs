namespace FinanceApp.Core.Models;

public class Transaction
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }

}
