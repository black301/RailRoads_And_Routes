using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using NuGet.Protocol.Core.Types;
using Stripe;
using System.Security.Claims;
using Transport__system_prototype.Models;
using Transport__system_prototype.Repository;
using Transport__system_prototype.Services;  
using Transport__system_prototype.ViewModels;
using System.Text.Json;

namespace Transport__system_prototype.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IEmailSender emailSender;

        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailSender = emailSender;
        }
        //register
        public IActionResult RegisterView()
        {
            var googleData = TempData["GoogleRegisterData"]?.ToString();
            if (googleData != null)
            {
                var registerVM = JsonSerializer.Deserialize<RegisterVM>(googleData);
                return View("Register", registerVM);
            }
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
        public IActionResult LoginView()
        {
            return View("Login");
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
        public IActionResult LoginGoogle()
        {
            var properties = new AuthenticationProperties 
            { 
                RedirectUri = Url.Action("GoogleResponse", "Account") 
            };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            if (!result.Succeeded)
                return RedirectToAction("Login");

            var googleUser = result.Principal;
            var email = googleUser.FindFirstValue(ClaimTypes.Email);
            var name = googleUser.FindFirstValue(ClaimTypes.Name);
            var firstName = googleUser.FindFirstValue(ClaimTypes.GivenName);
            var lastName = googleUser.FindFirstValue(ClaimTypes.Surname);

            // Check if user exists
            var user = await userManager.FindByEmailAsync(email);
            if (user != null)
            {
                // User exists, sign them in
                await signInManager.SignInAsync(user, false);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // User doesn't exist, redirect to register with pre-filled data
                var registerVM = new RegisterVM
                {
                    Email = email,
                    FullName = name,
                    UserName = email.Split('@')[0], // Create username from email
                };

                // Store the VM in TempData
                TempData["GoogleRegisterData"] = JsonSerializer.Serialize(registerVM);
                return RedirectToAction("RegisterView");
            }
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
        // Edit Profile View
        public async Task<IActionResult> EditProfile()
        {
            // Get the currently logged-in user
            var user = await userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login");
            }

            // Create a view model populated with the user's current data
            var profileVM = new EditProfileVM
            {
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                City = user.City,
                FullName = user.FullName
            };  

            return View(profileVM);
        }

        // Edit Profile POST (Handle Form Submission)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(EditProfileVM profileVM)
        {
            if (ModelState.IsValid)
            {
                // Get the currently logged-in user
                var user = await userManager.GetUserAsync(User);

                if (user == null)
                {
                    return RedirectToAction("Login");
                }

                // Update user's information
                user.UserName = profileVM.UserName;
                user.Email = profileVM.Email;
                user.PhoneNumber = profileVM.PhoneNumber;
                user.City = profileVM.City;
                user.FullName = profileVM.FullName;

                var result = await userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home"); // Redirect to a different view (e.g., Home)
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View(profileVM); // Return to the form with error messages if any
        }


        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    
                    // Generate a random 6-digit code
                    Random random = new Random();
                    string resetCode = random.Next(100000, 999999).ToString();
                    
                    // Send email with the 6-digit code
                    await emailSender.SendEmailAsync(
                        model.Email,
                        "Password Reset Code",
                        $"Your password reset code is: {resetCode}"
                    );
                    
                    TempData["ResetEmail"] = model.Email;
                    TempData["ResetToken"] = token; 
                    TempData["ResetCode"] = resetCode;
                    return RedirectToAction("ResetPassword");
                }
                ModelState.AddModelError("", "Email not found");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPassword()
        {
            var email = TempData["ResetEmail"]?.ToString();
            var code = TempData["ResetCode"]?.ToString();
            
            // Preserve the values for the POST action
            TempData.Keep("ResetEmail");
            TempData.Keep("ResetCode");

            if (string.IsNullOrEmpty(email))
                return RedirectToAction(nameof(ForgotPassword));

            var model = new ResetPasswordViewModel { Email = email };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var storedCode = TempData["ResetCode"]?.ToString();
                
                if (model.Code != storedCode)
                {
                    ModelState.AddModelError("Code", "Invalid reset code");
                    return View(model);
                }

                var user = await userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    // Generate a new token for password reset
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    var result = await userManager.ResetPasswordAsync(user, token, model.NewPassword);
                    
                    if (result.Succeeded)
                    {
                        TempData["Success"] = "Password has been reset successfully";
                        return RedirectToAction(nameof(Login));
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }
    
    }
}

