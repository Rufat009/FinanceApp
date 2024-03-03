using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceApp.Core.Dtos
{
    public class LoginDto
    {
    public string ReturnUrl { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    }
}