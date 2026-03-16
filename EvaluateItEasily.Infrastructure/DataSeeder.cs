using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;


namespace EvaluateItEasily.Infrastructure
{
    public static class DataSeeder
    {
        public static async Task SeedRolesAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            foreach (var role in Enum.GetNames<UserRole>())
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}
