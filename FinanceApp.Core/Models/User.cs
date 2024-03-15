using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Core.Models;

public class User : IdentityUser
{
    public int? Age { get; set; }
    public string? Surname { get; set; }


}
