using Microsoft.AspNetCore.Identity;
using taskapi.Entities;
using taskapi.Statics;

namespace taskapi.Data;

public static  class AppDbSeed
{
  public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
    {
        using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
        {

            var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            if (!await roleManager.RoleExistsAsync(UserRoles.User))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.User));
            
            var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var adminUserName = "admin";
            var adminUser = await userManager.FindByNameAsync(adminUserName);

            if (adminUser == null)
            {
                var newAdminUser = new User()
                {
                    FirstName = "Admin",
                    UserName = adminUserName,
                    Email = "admin@gmail.com",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(newAdminUser, "admin123");
                await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
            }


            var appUserUserName = "user";
            var appUser = await userManager.FindByNameAsync(appUserUserName);

            if (appUser == null)
            {
                var newAppUser = new User()
                {
                    FirstName = "Application User",
                    UserName = appUserUserName,
                    Email = "user@gmail.com",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(newAppUser, "user123");
                await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
            }
        }
    }

    public static void SeedUsersAndRolesAsync()
    {
        throw new NotImplementedException();
    }
}