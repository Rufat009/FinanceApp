using FinanceApp.Core.Repositories;
using FinanceApp.Infrastructure.Respositories;
using FinanceApp.Middlewares;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

string? connectionString = builder.Configuration.GetConnectionString("FinanceAppDb");

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Identity/Login";
        options.ReturnUrlParameter = "returnUrl";
    });

builder.Services.AddScoped<ITransactionRepository>(p =>
{
    return new TransactionRepository(new SqlConnection(connectionString));
});

builder.Services.AddScoped<IUserRepository>(p =>
{
    return new UserRepository(new SqlConnection(connectionString));
});

builder.Services.AddScoped<ILogRepository>(p =>
{
    return new FinanceLogRepository(new SqlConnection(connectionString));
});

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
