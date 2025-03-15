using Microsoft.AspNetCore.Identity;
using WebAppStore.Models;

namespace WebAppStore.DataSeeder
{
    public static class Seeder
    {
        public static async Task SeedRolesAndUsers(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            // Define the roles
            string[] roleNames = { "Admin", "User" };

            // Create roles if they do not exist
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            
            // Create Admin user
            var adminEmail = "admin@example.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new AppUser
                {
                    Name = "Admin",
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                string adminPassword = "Admin@123";
                var result = await userManager.CreateAsync(adminUser, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
            // Create Admin user
            var userEmail = "user@example.com";
            var User = await userManager.FindByEmailAsync(userEmail);

            if (User == null)
            {
                User = new AppUser
                {
                    Name = "User",
                    UserName = userEmail,
                    Email = userEmail,
                    EmailConfirmed = true
                };

                string UserPassword = "User@123";
                var result = await userManager.CreateAsync(User, UserPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(User, "User");
                }
            }



        }
    }




}
