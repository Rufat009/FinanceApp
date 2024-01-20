using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BinanceApp.Controllers.Base;

public class ControllerBase
{
    public HttpListenerContext HttpContext;

    public string View([CallerMemberName] string MethodName = "")
    {
        var controllerName = this.GetType().Name[..(this.GetType().Name.LastIndexOf("Controller"))];

        return File.ReadAllText($"{controllerName}/{MethodName}.html");
    }

}
