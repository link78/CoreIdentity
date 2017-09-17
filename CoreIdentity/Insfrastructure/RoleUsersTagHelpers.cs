using CoreIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreIdentity.Insfrastructure
{
    [HtmlTargetElement("td", Attributes = "identity-role")]
    public class RoleUsersTagHelpers:TagHelper
    {
        private  UserManager<AppUser> userManager;
        private  RoleManager<IdentityRole> roleManager;

        
        public RoleUsersTagHelpers(UserManager<AppUser> _userManager, RoleManager<IdentityRole> _roleManager)
        {
            userManager = _userManager;
            roleManager = _roleManager;
        }

        [HtmlAttributeName("identity-role")]
        public string Role { get; set; }

        public override async Task ProcessAsync(TagHelperContext context , TagHelperOutput output)
        {
            List<string> list = new List<string>();
            IdentityRole role = await roleManager.FindByIdAsync(Role);

            if(role != null)
            {
                foreach(var user in userManager.Users)
                {
                    if(user !=null && await userManager.IsInRoleAsync(user, role.Name))
                    {
                        list.Add(user.UserName);
                    }
                }
            }

            output.Content.SetContent(list.Count == 0 ? "No Users" : string.Join(", ", list));
        }
    }
}
