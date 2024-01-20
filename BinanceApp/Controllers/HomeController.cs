using BinanceApp.Attributes;
using BinanceApp.Controllers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BinanceApp.Controllers;

public class HomeController : ControllerBase
{

    [HttpGet]
    public async Task HomePageAsync()
    {
        using var writer = new StreamWriter(base.HttpContext.Response.OutputStream);

        var pageHtml = await File.ReadAllTextAsync("Views/Home.html");
        await writer.WriteLineAsync(pageHtml);
        base.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
        base.HttpContext.Response.ContentType = "text/html";
    }
}
