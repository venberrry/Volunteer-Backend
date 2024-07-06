using MakeVolunteerGreatAgain.Persistence;
using MakeVolunteerGreatAgain.Web.Helpers;
using MakeVolunteerGreatAgain.Web.Properties;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Регистрация сервисов
// От циклов в JSON и для взаимосвязанных объектов

StartupHelpers.RegisterDomainServices(builder, builder);

// Добавление Swagger, JWT tokens, CORS 
SwaggerJwtConfigurator.StartupConfigurator(builder);

var app = builder.Build();

// Выполнение миграций
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

// Конфигурация HTTP-запросов
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MakeVolunteerGreatAgain API V1");
        c.InjectStylesheet("/swagger-ui/dark-theme.css");
    });
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
