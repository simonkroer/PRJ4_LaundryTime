using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaundryTime.Data;
using LaundryTime.Data.Models;
using LaundryTime.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using LaundryTime.Areas.Identity.Pages.Account;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity.UI.Services;
using static LaundryTime.ViewModels.SystemAdminViewModel;
using Microsoft.AspNetCore.Authentication;

namespace LaundryTime.Controllers
{
    [Authorize(Policy="IsSystemAdmin")]
	public class SystemAdminController : Controller
	{

        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _context;
        private IDataAccessAction _dataAccess;
        public SystemAdminViewModel _systemAdminViewModel;

        public IList<AuthenticationScheme> ExternalLogins { get; set; }


        public SystemAdminController(UserManager<ApplicationUser> userManager,
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
			_systemAdminViewModel = new SystemAdminViewModel();
		}



		public IActionResult Index()
		{
				_systemAdminViewModel.AllUserAdmins = _dataAccess.UserAdmins.GetAllUserAdmins();
				return View(_systemAdminViewModel);
		}

        public IActionResult CreateUserAdmin()
        {
            return View(_systemAdminViewModel);
        }
        
        //=============================Create UserAdmin=========================================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUserAdmin(SystemAdminViewModel vm)
        {
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {

                //Til systemadmin til at oprette de forskellige typer brugere
                    var user = new UserAdmin()
                    {
                        UserName = vm.Input.Email,
                        Email = vm.Input.Email,
                        Name = vm.Input.Name,
                        WorkAddress = new Address() { StreetAddress = vm.Input.StreetAddress, Zipcode = vm.Input.Zipcode },
                        PhoneNumber = vm.Input.Phonenumber,
                        PaymentMethod = vm.Input.PaymentMethod
                    };

                    var result = await _userManager.CreateAsync(user, vm.Input.Password);

                    if (result.Succeeded)
                    {
                        _logger.LogInformation("UserAdmin is created! ");

                        if (User.Identity != null)
                        {
                            var systemadmin = _dataAccess.SystemAdmins.GetSingleSystemAdmin(User.Identity.Name);
                            systemadmin.UserAdmins.Add(user);
                            _dataAccess.Complete();
                        }

                        //if (_userManager.Options.SignIn.RequireConfirmedAccount)
                        //{
                        //    return RedirectToPage("RegisterConfirmation", new { email = vm.Input.Email, returnUrl = returnUrl });
                        //}
                        //else
                        //{
                        //    await _signInManager.SignInAsync(user, isPersistent: false);
                        //    return LocalRedirect(returnUrl);
                        //}
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
            }
            return RedirectToAction(nameof(Index));
        }

        //Get UserAdminDetails=================================================================
        public async Task<IActionResult> UserAdminDetails(string username)
        {
            if(username == null)
            {
                return NotFound();
            }

            var userAdmin = _dataAccess.UserAdmins.GetSingleUserAdmin(username);
            if (userAdmin == null)
            {
                return NotFound();
            }
            return View(userAdmin);
        }



    }


}