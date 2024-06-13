using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using VolunteerProject.DataBase;
using VolunteerProject.Models;

var builder = WebApplication.CreateBuilder(args);

// Добавление контекста базы данных
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Добавление служб для управления идентификацией
builder.Services.AddIdentity<User, IdentityRole<int>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Добавление служб
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Добавление Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Volunteer API", Version = "v1" });
});

var app = builder.Build();

// Конфигурация HTTP-запросов
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Volunteer API v1");
        c.RoutePrefix = string.Empty; // Swagger UI будет доступен на корневом URL (например, http://localhost:<port>/)
    });
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
    app.UseSwagger(); // Добавьте это для использования Swagger в production
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Volunteer API v1");
        c.RoutePrefix = string.Empty; // Swagger UI будет доступен на корневом URL (например, http://localhost:<port>/)
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();