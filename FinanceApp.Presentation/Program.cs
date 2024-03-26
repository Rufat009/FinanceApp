using System.Reflection;
using FinanceApp.Core.Models;
using FinanceApp.Core.Repositories;
using FinanceApp.Infrastructure.Data;
using FinanceApp.Infrastructure.Repositories;
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

builder.Services.ConfigureApplicationCookie( p => {
    p.LoginPath = "/Identity/Login";

});

builder.Services.AddScoped<IServiceRepository,ServiceRepository>();

builder.Services.AddScoped<IBillRepository,BillRepository>();



builder.Services.AddScoped<FinanceAppDbContext>();

builder.Services.AddScoped<ILogRepository, FinanceLogRepository>();

builder.Services.AddTransient<LogMiddleware>();

var app = builder.Build();

app.UseAuthentication();

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
