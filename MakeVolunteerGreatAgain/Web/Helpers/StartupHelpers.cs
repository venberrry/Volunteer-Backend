using MakeVolunteerGreatAgain.Core.Entities;
using MakeVolunteerGreatAgain.Core.Repositories;
using MakeVolunteerGreatAgain.Core.Services;
using MakeVolunteerGreatAgain.Infrastructure.Services;
using MakeVolunteerGreatAgain.Infrastructure.Services.Redis;
using MakeVolunteerGreatAgain.Infrastructure.Services.Token;
using MakeVolunteerGreatAgain.Infrastructure.Services.Transfer;
using MakeVolunteerGreatAgain.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace MakeVolunteerGreatAgain.Web.Helpers;

public static class StartupHelpers
{
    
    public static void RegisterDomainServices(WebApplicationBuilder webApplicationBuilder, WebApplicationBuilder builder)
    {
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
        
        builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
            options.JsonSerializerOptions.MaxDepth = 6;
        });

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
        builder.Services.AddScoped<IApplicationService, ApplicationService>();


        // Регистрация Redis
        var redisConfiguration = builder.Configuration.GetSection("Redis:Configuration").Value;
        var redisInstanceName = builder.Configuration.GetSection("Redis:InstanceName").Value;
        var multiplexer = ConnectionMultiplexer.Connect(redisConfiguration);

        builder.Services.AddSingleton<IConnectionMultiplexer>(multiplexer);
        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConfiguration;
            options.InstanceName = redisInstanceName;
        });

        builder.Services.AddScoped<ICacheService, RedisCacheService>();
    }
}