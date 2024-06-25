using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using VolunteerProject.DataBase;
using VolunteerProject.Models;
using VolunteerProject.Services;
using VolunteerProject.Services.Events;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using VolunteerProject.AppConfigurations;
using VolunteerProject.Helpers;
using VolunteerProject.Services.Subscription;

var builder = WebApplication.CreateBuilder(args);

// Добавление контекста базы данных
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Регистрация сервисов
StartupHelpers.RegisterDomainServices(builder, builder);
// Конфигурация JWT
AppSecurityConfigurator.StartupConfigurator(builder);

// Добавление служб
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Конфигурация HTTP-запросов
SwaggerConfig.SwaggerConfigurator(app);
// Конфигурация ролей
await RoleConfig.RoleConfigurator(app);

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowSpecificOrigin"); // Обратите внимание, что политика называется "AllowSpecificOrigin"
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();
app.Run();