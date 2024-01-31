using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceApp.Models;

public class Cryptocurrency
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public double? Count { get; set; }
    public double? Price { get; set; }

}
