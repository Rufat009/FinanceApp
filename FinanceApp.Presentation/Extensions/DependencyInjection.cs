using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceApp.Core.Repositories;
using FinanceApp.Core.Services;
using FinanceApp.Infrastructure.Data;
using FinanceApp.Infrastructure.Repositories;
using FinanceApp.Infrastructure.Respositories;
using FinanceApp.Infrastructure.Services;
using FinanceApp.Middlewares;

namespace FinanceApp.Presentation.Extensions;

public static class DependencyInjection
{
    public static void Inject(this IServiceCollection services)
    {
        services.AddScoped<IServiceRepository, ServiceRepository>();

        services.AddScoped<IServiceService, ServiceService>();

        services.AddScoped<IBillService, BillService>();

        services.AddScoped<IBillRepository, BillRepository>();

        services.AddScoped<IUserService, UserService>();

        services.AddScoped<FinanceAppDbContext>();

        services.AddScoped<ILogRepository, FinanceLogRepository>();

        services.AddTransient<LogMiddleware>();
    }
}
