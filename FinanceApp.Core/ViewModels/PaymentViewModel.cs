using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceApp.Core.Models;

namespace FinanceApp.Core.ViewModels
{
    public class PaymentViewModel
    {
        public Service Service { get; set; }
        public User User { get; set; }
    }
}