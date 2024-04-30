using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace FinanceApp.Core.Dtos
{
    public class ServiceDto
    {
        public IFormFile ServiceImageUrl { get; set; }
        public string ServiceName { get; set; }
        public double ServiceCost { get; set; }

    }
}