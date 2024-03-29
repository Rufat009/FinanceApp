
using System.Text;
using FinanceApp.Core.Models;
using FinanceApp.Core.Repositories;
using Microsoft.AspNetCore.Http.Extensions;


namespace FinanceApp.Middlewares
{
    public class LogMiddleware : IMiddleware
    {
        private ILogRepository logRepository;
        private readonly IConfiguration isLogging;
        public LogMiddleware(IConfiguration isLogging, ILogRepository logRepository)
        {
            this.isLogging = isLogging;
            this.logRepository = logRepository;
        }
        public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
        {
            if (!isLogging.GetSection("ToLog").Get<bool>())
            {
                await next.Invoke(httpContext);
            }
            else
            {
                var methodType = httpContext.Request.Method;

                var url = httpContext.Request.GetDisplayUrl();


                var requestBody = string.Empty;

                if (httpContext.Request.Body.CanRead)
                {
                    if (!httpContext.Request.Body.CanSeek)
                    {
                        httpContext.Request.EnableBuffering();
                    }

                    httpContext.Request.Body.Position = 0;

                    StreamReader requestReader = new(httpContext.Request.Body, Encoding.UTF8);

                    requestBody = await requestReader.ReadToEndAsync();

                    httpContext.Request.Body.Position = 0;
                }

                var responseBody = string.Empty;

                Stream originalBody = httpContext.Response.Body;

                using (var memStream = new MemoryStream())
                {
                    httpContext.Response.Body = memStream;

                    await next.Invoke(httpContext);

                    memStream.Position = 0;

                    StreamReader responseReader = new(httpContext.Response.Body, Encoding.UTF8);

                    responseBody = await responseReader.ReadToEndAsync();

                    memStream.Position = 0;

                    await memStream.CopyToAsync(originalBody);
                }

                var statusCode = httpContext.Response.StatusCode;

                httpContext.Response.Body = originalBody;

                await logRepository.CreateAsync(new Log
                {
                    UserId = default,
                    Url = url,
                    MethodType = methodType,
                    StatusCode = statusCode,
                    RequestBody = requestBody,
                    ResponseBody = responseBody
                }
                );

            }
        }
    }
}