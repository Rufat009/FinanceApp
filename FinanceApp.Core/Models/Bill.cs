using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceApp.Core.Models
{
    public class Bill
    {
        public int Id {get; set;}
        public DateTime PayDate {get; set;}
        public Service? Service {get;set;} 
        public User? User {get;set;}
        public double AmountSpent {get; set;}
        
    }
}