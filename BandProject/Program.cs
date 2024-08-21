using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BandProject.Data;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSession(option => { option.IdleTimeout = TimeSpan.FromHours(24); });
builder.Services.AddDbContext<BandProjectContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("BandProjectContext") ?? throw new InvalidOperationException("Connection string 'BandProjectContext' not found."))); ; ; ; ;;
builder.Services.AddControllersWithViews();
var app = builder.Build();

app.UseStaticFiles();
app.UseSession();
app.MapDefaultControllerRoute();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Tour}/{action=Index}/{id?}");

app.Run();
