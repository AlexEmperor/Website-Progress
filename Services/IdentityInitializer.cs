using Microsoft.AspNetCore.Identity;
using Website_Progress.ModelsDTO;

namespace Website_Progress.Services
{
    public class IdentityInitializer
    {
        public static void Inititalize(UserManager<UserDTO> userManager, RoleManager<IdentityRole> roleManager)
        {
            var adminEmail = "admin@gmail.com";
            var password = "_Aa123456";

            if (roleManager.FindByNameAsync(Constants.AdminRoleName).Result == null)
            {
                roleManager.CreateAsync(new IdentityRole(Constants.AdminRoleName)).Wait();
            }
            if (roleManager.FindByNameAsync(Constants.UserRoleName).Result == null)
            {
                roleManager.CreateAsync(new IdentityRole(Constants.UserRoleName)).Wait();
            }
            if (roleManager.FindByNameAsync(adminEmail).Result == null)
            {
                var admin = new UserDTO { Email = adminEmail, UserName = adminEmail };
                var result = userManager.CreateAsync(admin, password).Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(admin, Constants.AdminRoleName).Wait();
                }
            }
        }
    }
}
