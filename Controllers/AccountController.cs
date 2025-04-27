using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using NuGet.Protocol.Core.Types;
using Stripe;
using Transport__system_prototype.Models;
using Transport__system_prototype.Repository;
using Transport__system_prototype.ViewModels;

namespace Transport__system_prototype.Controllers
{
    public class AccountController : Controller
    {
        //DI
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        //register
        public IActionResult RegisterView()
        {
            return View("Register");
        }
        //register post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM VM)
        {
            if (ModelState.IsValid)
            {
                // Check if email already exists
                var existingUser = await userManager.FindByEmailAsync(VM.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Email is already registered.");
                    return View(VM);
                }

                // Create a new user
                AppUser user = new AppUser
                {
                    UserName = VM.UserName,
                    Email = VM.Email,
                    PhoneNumber = VM.PhoneNumber,
                    City = VM.City,
                    FullName = VM.FullName
                };

                var isGoodUser = await userManager.CreateAsync(user, VM.Password);
                if (isGoodUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Client");
                    return View("Login");
                }
                else
                {
                    foreach (var error in isGoodUser.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View(VM);
        }
        //login
        public IActionResult Login()
        {
            return View();
        }
        //login post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM VM)
        {
            if (ModelState.IsValid)
            {
                //find the user by email
                var user = await userManager.FindByEmailAsync(VM.Email);
                if (user != null)
                {
                    //check the password
                    var isGoodUser = await userManager.CheckPasswordAsync(user, VM.Password);
                    if (isGoodUser)
                    {
                        //sign in the user
                        await signInManager.SignInAsync(user, VM.RememberMe);
                        return RedirectToAction("index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Wrong Data");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Wrong Data");
                }
            }
            return View(VM);
        }
        public IActionResult LoginGoolge()
        {
            var redirectUrl = Url.Action("Index", "Home");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }
        //logout
        public async Task<IActionResult> Logout()
        {
            //sign out the user
            await signInManager.SignOutAsync();
            return RedirectToAction("index", "Home");
        }
        //access denied
        public IActionResult AccessDenied(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

    }
}
