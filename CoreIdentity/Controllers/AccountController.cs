using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CoreIdentity.Models;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CoreIdentity.Controllers
{

    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signManager;
        private readonly IUserValidator<AppUser> userValidator;
        private readonly IPasswordHasher<AppUser> passwordHasher;
        private readonly IPasswordValidator<AppUser> passwordValidator;

        public AccountController(UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager,
            IUserValidator<AppUser> _userValidator, IPasswordValidator<AppUser> _passwordValidator, IPasswordHasher<AppUser> _passwordHasher)
        {
            userManager = _userManager;
            signManager = _signInManager;
            userValidator = _userValidator;
            passwordHasher = _passwordHasher;
            passwordValidator = _passwordValidator;
        }




        //Displaying
        public IActionResult Index()
        {
            return View(userManager.Users);
        }


        public IActionResult Register()
        {
            return View();
        }


        //Creating new user
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel rv)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    UserName = rv.Name,
                    Email = rv.Email,
                };

                var result = await userManager.CreateAsync(user, rv.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    AddErrorFromResult(result);
                }

            }
            return View(rv);
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }


        //creating login
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task< IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    //await signManager.SignOutAsync();
                    var result = await signManager.PasswordSignInAsync(user, model.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                


             ModelState.AddModelError("", "Invalid Email or Password");
                
                

                  
            }

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
            














        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            if (ModelState.IsValid)
            {
                //creating a user
                var user = await userManager.FindByIdAsync(id);
                if(user != null)
                {
                    var result = await userManager.DeleteAsync(user);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        AddErrorFromResult(result);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Uers not found");
                }
            }
            return View("Index", userManager.Users);
        }


        public async Task<IActionResult> Edit(string id)
        {
            AppUser user = await userManager.FindByIdAsync(id);
            if(user != null)
            {
                return View(user);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, string email, string password)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.Email = email;
                IdentityResult validEmail = await userValidator.ValidateAsync(userManager, user);
                if (!validEmail.Succeeded)
                {
                    AddErrorFromResult(validEmail);
                }

                IdentityResult validPass = null;
                if (!string.IsNullOrEmpty(password))
                {
                    validPass = await passwordValidator.ValidateAsync(userManager, user, password);
                    if (validPass.Succeeded)
                    {
                        user.PasswordHash = passwordHasher.HashPassword(user, password);
                    }
                    else
                    {
                        AddErrorFromResult(validPass);
                    }
                }

                if ((validPass.Succeeded && validEmail == null) || (validEmail.Succeeded && password != string.Empty && validPass.Succeeded))
                {
                    var result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        AddErrorFromResult(result);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "User not found");

            }
            return View(user);
        }











        // void method for method
        private void AddErrorFromResult(IdentityResult result)
        {
            foreach( IdentityError errors in result.Errors)
            {
                ModelState.AddModelError("", errors.Description);
            }
        }
    }
}
