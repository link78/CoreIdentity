using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//namespace CoreIdentity.Models
//{
//    public  class DbInitializer 
//    {
       

//        public static async Task CreateAdmin(IServiceProvider service)
//        {
//            UserManager<AppUser> userManager = service.GetRequiredService<UserManager<AppUser>>();
//            RoleManager<IdentityRole> roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();

//            string username = "Admin";
//            string email = "AdminG@example.com";
//            string pass = "Secrete90";
//            string role = "Admins";

//            if(await userManager.FindByNameAsync(username)== null)
//            {
//                if(await roleManager.FindByNameAsync(role)== null)
//                {
//                    await roleManager.CreateAsync(new IdentityRole(role));
//                }
//                var user = new AppUser { UserName = username, Email = email };

//                var result = await userManager.CreateAsync(user, pass);
//                if (result.Succeeded) { await userManager.AddToRoleAsync(user, role); }
//            }

//        }
        
//    }

    
}
