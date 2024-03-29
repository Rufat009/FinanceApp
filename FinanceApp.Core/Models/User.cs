using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Core.Models;

public class User : IdentityUser
{
    public int? Age { get; set; }
    public string? Surname { get; set; }
    public double? Balance { get; set; } = 0;
    public int AbonentNumber {get; set; }
}
