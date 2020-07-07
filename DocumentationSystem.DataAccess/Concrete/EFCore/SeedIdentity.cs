using DocumentationSystem.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DocumentationSystem.DataAccess.Concrete.EFCore
{
    public class SeedIdentity
    {
        public static async Task Seed(UserManager<DocSysUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            var username = configuration["Data:AdminUser:username"];
            var email = configuration["Data:AdminUser:email"];
            var password = configuration["Data:AdminUser:password"];
            var role = configuration["Data:AdminUser:role"];


            if (await userManager.FindByEmailAsync(username) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(role));
                await roleManager.CreateAsync(new IdentityRole("user"));
                var user = new DocSysUser()
                {
                    UserName = username,
                    Email = email,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
        }
    }
}
