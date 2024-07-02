using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MakeVolunteerGreatAgain.Core.Entities;
using MakeVolunteerGreatAgain.Persistence;
using MakeVolunteerGreatAgain.Infrastructure.Services;
using MakeVolunteerGreatAgain.Core.Services;
using MakeVolunteerGreatAgain.Core.Repositories;
using MakeVolunteerGreatAgain.Infrastructure.Services.Token;
using MakeVolunteerGreatAgain.Web.Properties;

var builder = WebApplication.CreateBuilder(args);

// Добавление конфигурации из файла appsettings.json
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("Web/appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

// Добавление контекста базы данных
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Регистрация служб Identity
builder.Services.AddIdentity<CommonUser, IdentityRole<int>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Регистрация RoleInitializer
builder.Services.AddScoped<RoleInitializer>();

// Регистрация репозиториев и сервисов
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IVolunteerRepository, VolunteerRepository>();
builder.Services.AddScoped<IOrganizationRepository, OrganizationRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IInvitationService, InvitationService>();
builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();

// Добавление Swagger, JWT tokens, CORS 
SwaggerJwtConfigurator.StartupConfigurator(builder);

// От циклов в JSON и для взаимосвязанных объектов
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});

// Добавление служб для контроллеров
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Конфигурация HTTP-запросов
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MakeVolunteerGreatAgain API V1"));
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();

// Инициализация ролей
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleInitializer = services.GetRequiredService<RoleInitializer>();
    await roleInitializer.InitializeAsync();
}

// Настройка маршрутов
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
