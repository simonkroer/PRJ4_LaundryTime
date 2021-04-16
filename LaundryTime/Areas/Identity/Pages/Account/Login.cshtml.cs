using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using LaundryTime.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace LaundryTime.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<LaundryUser> _userManagerlaundryuser;
        private readonly UserManager<UserAdmin> _userManageruseradmin;
        private readonly UserManager<SystemAdmin> _userManagersystemadmin;
        private readonly SignInManager<LaundryUser> _signInManagerlaundryuser;
        private readonly SignInManager<UserAdmin> _signInManageruseradmin;
        private readonly SignInManager<SystemAdmin> _signInManagersystemadmin;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<LaundryUser> signInManagerlaundryuser,
            SignInManager<UserAdmin> signInManageruseradmin,
            SignInManager<SystemAdmin> signInManagersystemadmin,
            ILogger<LoginModel> logger,
            UserManager<LaundryUser> userManagerlaundryuser,
                UserManager<UserAdmin> userManageruseradmin,
            UserManager<SystemAdmin> userManagersystemadmin)
        {
         _userManagerlaundryuser = userManagerlaundryuser;
        _userManageruseradmin = userManageruseradmin;
        _userManagersystemadmin = userManagersystemadmin;
        _signInManagerlaundryuser = signInManagerlaundryuser;
        _signInManageruseradmin = signInManageruseradmin;
        _signInManagersystemadmin = signInManagersystemadmin;
        _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            if (User.HasClaim("LaundryUser", "IsLaundryUser"))
            {
                ExternalLogins = (await _signInManagerlaundryuser.GetExternalAuthenticationSchemesAsync()).ToList(); 
            }
            if (User.HasClaim("UserAdmin", "IsUserAdmin"))
            {

                ExternalLogins = (await _signInManageruseradmin.GetExternalAuthenticationSchemesAsync()).ToList();
            }
            if (User.HasClaim("SystemAdmin", "IsSystemAdmin"))
            {

                ExternalLogins = (await _signInManagersystemadmin.GetExternalAuthenticationSchemesAsync()).ToList();
            }


            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            

            if (User.HasClaim("LaundryUser", "IsLaundryUser"))
            {
                ExternalLogins = (await _signInManagerlaundryuser.GetExternalAuthenticationSchemesAsync()).ToList();

                if (ModelState.IsValid)
                {
                    // This doesn't count login failures towards account lockout
                    // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                    var result = await _signInManagerlaundryuser.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User logged in.");
                        return LocalRedirect(returnUrl);
                    }
                    if (result.RequiresTwoFactor)
                    {
                        return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                    }
                    if (result.IsLockedOut)
                    {
                        _logger.LogWarning("User account locked out.");
                        return RedirectToPage("./Lockout");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        return Page();
                    }
                }
            }

            if (User.HasClaim("UserAdmin", "IsUserAdmin"))
            {
                ExternalLogins = (await _signInManageruseradmin.GetExternalAuthenticationSchemesAsync()).ToList();

                if (ModelState.IsValid)
                {
                    // This doesn't count login failures towards account lockout
                    // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                    var result = await _signInManageruseradmin.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User logged in.");
                        return LocalRedirect(returnUrl);
                    }
                    if (result.RequiresTwoFactor)
                    {
                        return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                    }
                    if (result.IsLockedOut)
                    {
                        _logger.LogWarning("User account locked out.");
                        return RedirectToPage("./Lockout");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        return Page();
                    }
                }

            }

            if (User.HasClaim("SystemAdmin", "IsSystemAdmin"))
            {
                ExternalLogins = (await _signInManagersystemadmin.GetExternalAuthenticationSchemesAsync()).ToList();

                if (ModelState.IsValid)
                {
                    // This doesn't count login failures towards account lockout
                    // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                    var result = await _signInManagersystemadmin.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User logged in.");
                        return LocalRedirect(returnUrl);
                    }
                    if (result.RequiresTwoFactor)
                    {
                        return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                    }
                    if (result.IsLockedOut)
                    {
                        _logger.LogWarning("User account locked out.");
                        return RedirectToPage("./Lockout");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        return Page();
                    }
                }

            }

            

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
