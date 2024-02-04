using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Controllers
{
    public class FinanceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
