using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceApp.Core.Dtos
{
    public class UserDto
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public int? Age { get; set; }
        public decimal? Balance { get; set; }
        public string? Email { get; set;}
        public string? Password { get; set; }
        public int AbonentNumber {get; set; }
    }
}