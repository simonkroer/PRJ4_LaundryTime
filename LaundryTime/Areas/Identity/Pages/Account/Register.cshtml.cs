﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using LaundryTime.Data;
using LaundryTime.Data.Models;
using LaundryTime.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace LaundryTime.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _context;
        private IDataAccessAction _dataAccess;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender, 
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
            _dataAccess = new DataAccsessAction(context);
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
            [Display(Name = "Name")]
            public string Name { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
            [Display(Name = "Street Address")]
            public string StreetAddress { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 4)]
            [Display(Name = "Postal Code")]
            public string Zipcode { get; set; }

            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 8)]
            [Display(Name = "Phone no.")]
            public string Phonenumber { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 4)]
            [Display(Name = "Payment Method")]
            public string PaymentMethod { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                //Til useradmin til at oprette laundrytime brugere
                if (User.HasClaim("UserAdmin", "IsUserAdmin"))
                {
                    var user = new LaundryUser { UserName = Input.Email, Email = Input.Email, Name = Input.Name, 
                        Address = new Address(){StreetAddress = Input.StreetAddress, Zipcode = Input.Zipcode}, 
                        PhoneNumber = Input.Phonenumber,PaymentMethod = Input.PaymentMethod};

                    var result = await _userManager.CreateAsync(user, Input.Password);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User created a new account with password.");

                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                            protocol: Request.Scheme);

                        MailMessage message = new MailMessage()
                        {
                            From = new MailAddress("laundrytime@outlook.dk"), // sender must be a full email address
                            Subject = "Please confirm your e-mail",
                            IsBodyHtml = true,
                            Body = $"<h3>Hello {user.Name}</h3><p>Thank you for registering with Laundry Time!</p> " +
                                   $"<p>Below you will find your user information:</p> <p>User name: {user.Email} </p> " +
                                   $"<p>Password: {Input.Password} </p> " +
                                   $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>. " +
                                   $"<br/> <br/> <img width='100' src='https://t4.ftcdn.net/jpg/03/09/29/23/360_F_309292393_4G7XxgXz5ftKSuSStItdT2ZK1snVEH08.jpg'/> <p>Kind regards</p> <p>Laundry Time</p>",
                            BodyEncoding = System.Text.Encoding.UTF8,
                            SubjectEncoding = System.Text.Encoding.UTF8,
                            To = { user.Email }
                        };

                        string smsMsg =
                            $"Hi {user.Name}!\n\nThank you for registering with Laundry Time!\n" +
                            $"Below you will find your user information:\nUser name: {user.Email}" +
                            $"\nPassword: {Input.Password}\n\n" +
                            $"Please remember to confirm your account by clicking the link in the mail sent to {user.Email}";

                          //SendMail(message);
                          //SendSMS(user.PhoneNumber, smsMsg);

                        if (User.Identity != null)
                        {
                            var useradmin = _dataAccess.UserAdmins.GetSingleUserAdmin(User.Identity.Name);
                            useradmin.Users.Add(user);
                            _dataAccess.Complete();
                        }

                        if (_userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                        }
                        else
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            return LocalRedirect(returnUrl);
                        }
                    }

	                    foreach (var error in result.Errors)
	                    {
	                        ModelState.AddModelError(string.Empty, error.Description);
	                    }
                }

                //Til systemadmin til at oprette de forskellige typer brugere
                if (User.HasClaim("SystemAdmin", "IsSystemAdmin"))
                {
	                var user = new UserAdmin() { UserName = Input.Email, Email = Input.Email, Name = Input.Name, 
		                WorkAddress = new Address() { StreetAddress = Input.StreetAddress, Zipcode = Input.Zipcode },
		                PhoneNumber = Input.Phonenumber,
		                PaymentMethod = Input.PaymentMethod
	                };

	                var result = await _userManager.CreateAsync(user, Input.Password);

	                if (result.Succeeded)
	                {
		                _logger.LogInformation("UserAdmin is created! ");

                        if (User.Identity != null)
                        {
                            var systemadmin = _dataAccess.SystemAdmins.GetSingleSystemAdmin(User.Identity.Name);
                            systemadmin.UserAdmins.Add(user);
                            _dataAccess.Complete();
                        }

                        if (_userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                        }
                        else
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            return LocalRedirect(returnUrl);
                        }
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    //=====================================================Hvad sker der her?==================================================================


                }

                //============================================================Hvad sker der her?===================================================================


                else
                {
                    var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email };
                    var result = await _userManager.CreateAsync(user, Input.Password);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User created a new account with password.");

                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                            protocol: Request.Scheme);

                        await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                        if (_userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                        }
                        else
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            return LocalRedirect(returnUrl);
                        }
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
