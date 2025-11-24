using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PortalNauchnyhPublikatsiy.Domain.Entities.Identity;
using System;
using System.Threading.Tasks;

namespace PortalNauchnyhPublikatsiy.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "Администратор", "Преподаватель", "Пользователь" };
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var adminEmail = "admin@portal.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var newAdmin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true 
                };

                var result = await userManager.CreateAsync(newAdmin, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, "Администратор");
                }
            }
        }
    }
}