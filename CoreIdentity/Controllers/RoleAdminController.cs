using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using CoreIdentity.Models;
using System.Data;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CoreIdentity.Controllers
{
    [Authorize]
    public class RoleAdminController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<AppUser> userManager;

        public RoleAdminController(RoleManager<IdentityRole> _roleManager, UserManager<AppUser> _userManager)
        {
            roleManager = _roleManager;
            userManager = _userManager;
        }


        public IActionResult Index()
        {
            return View(roleManager.Roles);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task< IActionResult> Create([Required] string name)
        {
            if (ModelState.IsValid)
            {
                var result = await roleManager.CreateAsync(new IdentityRole(name));

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrorFromResult(result);
                }
            }
            return View(name);
        }

        [HttpPost]
        public async Task<IActionResult>Delete(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if(role != null)
            {
                var result = await roleManager.DeleteAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                ModelState.AddModelError("", "Role not Found");
            }

            return View("Index", roleManager.Roles);
        }

       
       //Edit

        public async Task<IActionResult> Edit(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            var members = new List<AppUser>();
            var nonMembers = new List<AppUser>();

            foreach(AppUser user in userManager.Users)
            {
                var list = await userManager.IsInRoleAsync(user, role.Name) ? members : nonMembers;

                list.Add(user);
            }

            return View(new RoleViewModel
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            });
        }


        [HttpPost]
        public async Task<IActionResult> Edit(RoleModificationModel model)
        {
            IdentityResult result;
            if (ModelState.IsValid)
            {
                foreach(string userID in model.IdsToAdd ?? new string[] { })
                {
                    var user = await userManager.FindByIdAsync(userID);
                    if(user != null)
                    {
                        result = await userManager.AddToRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                        {
                            AddErrorFromResult(result);
                        }
                    }
                }

                //emoving from
                foreach (string userID in model.IdsToDelete ?? new string[] { })
                {
                    var user = await userManager.FindByIdAsync(userID);
                    if (user != null)
                    {
                        result = await userManager.RemoveFromRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                        {
                            AddErrorFromResult(result);
                        }
                    }
                }
            }
            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return await Edit(model.RoleId);
            }

        }



        private void AddErrorFromResult(IdentityResult result)
        {
            foreach (IdentityError errors in result.Errors)
            {
                ModelState.AddModelError("", errors.Description);
            }
        }
    }
}
