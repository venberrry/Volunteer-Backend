using Microsoft.AspNetCore.Identity;

namespace VolunteerProject.AppConfigurations;

public static class RoleConfig
{
    public static async Task RoleConfigurator(WebApplication webApplication)
    {
        using (var scope = webApplication.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();

            var roles = new[] { "Volunteer", "Organization" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole<int>(role));
                }
            }
        }
    }
}