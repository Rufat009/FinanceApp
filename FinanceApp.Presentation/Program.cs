using System.Reflection;
using FinanceApp.Core.Models;
using FinanceApp.Core.Repositories;
using FinanceApp.Infrastructure.Data;
using FinanceApp.Infrastructure.Respositories;
using FinanceApp.Middlewares;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

string? connectionString = builder.Configuration.GetConnectionString("FinanceAppDb");
builder.Services.AddDbContext<FinanceAppDbContext>(options =>
{

    options.UseSqlServer(connectionString, o =>
    {
        o.MigrationsAssembly("FinanceApp.Presentation");
    });
});


builder.Services.AddIdentity<User, IdentityRole>(
).AddEntityFrameworkStores<FinanceAppDbContext>();




builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Identity/Login";
        options.ReturnUrlParameter = "returnUrl";
    });

builder.Services.AddScoped<ITransactionRepository,TransactionRepository>();



builder.Services.AddScoped<FinanceAppDbContext>();

builder.Services.AddScoped<ILogRepository, FinanceLogRepository>();

builder.Services.AddTransient<LogMiddleware>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseMiddleware<LogMiddleware>();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
