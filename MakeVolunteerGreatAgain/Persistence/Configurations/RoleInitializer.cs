using Microsoft.AspNetCore.Identity;

namespace MakeVolunteerGreatAgain.Persistence
{
    public class RoleInitializer
    {
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public RoleInitializer(RoleManager<IdentityRole<int>> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task InitializeAsync()
        {
            string[] roleNames = { "Volunteer", "Organization" };
            foreach (var roleName in roleNames)
            {
                var roleExist = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await _roleManager.CreateAsync(new IdentityRole<int> { Name = roleName });
                }
            }
        }
    }
}