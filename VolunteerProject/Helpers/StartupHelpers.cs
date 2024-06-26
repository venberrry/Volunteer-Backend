using Microsoft.AspNetCore.Identity;
using VolunteerProject.DataBase;
using VolunteerProject.Models;
using VolunteerProject.Services;
using VolunteerProject.Services.Auth;
using VolunteerProject.Services.Events;
using VolunteerProject.Services.Invitation;
using VolunteerProject.Services.Subscription;

namespace VolunteerProject.Helpers;

public static class StartupHelpers
{
    
    public static void RegisterDomainServices(WebApplicationBuilder webApplicationBuilder, WebApplicationBuilder builder)
    {
        // Добавление служб для управления идентификацией
        builder.Services.AddIdentity<User, IdentityRole<int>>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        // Регистрация UserManager и RoleManager для Volunteer
        builder.Services.AddIdentityCore<Volunteer>()
            .AddRoles<IdentityRole<int>>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();


        // Регистрация UserManager и RoleManager для Organization
        builder.Services.AddIdentityCore<Organization>()
            .AddRoles<IdentityRole<int>>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        // Регистрация services
        webApplicationBuilder.Services.AddScoped<UserManager<Organization>>();
        webApplicationBuilder.Services.AddScoped<RoleManager<IdentityRole<int>>>();
        webApplicationBuilder.Services.AddScoped<IAuthService, AuthService>();
        webApplicationBuilder.Services.AddScoped<IInvitationService, InvitationService>();
        webApplicationBuilder.Services.AddScoped<UserManager<Volunteer>>();
        webApplicationBuilder.Services.AddScoped<RoleManager<IdentityRole<int>>>();
        webApplicationBuilder.Services.AddScoped<IEventService, EventService>();
        webApplicationBuilder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
        webApplicationBuilder.Services.AddScoped<IEmailService, EmailService>();
    }
}