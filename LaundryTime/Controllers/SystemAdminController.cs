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
using System.Security.Claims;

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
            _systemAdminViewModel.AllUsers = _dataAccess.LaundryUsers.GetAllLaundryUsers();
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

                        await _userManager.AddClaimAsync(user, new Claim("UserAdmin", "IsUserAdmin"));

                        _logger.LogInformation("UserAdmin is created! ");

                        var systemadmin = await _dataAccess.SystemAdmins.GetSingleSystemAdminAsync(User.Identity.Name);
                        systemadmin.UserAdmins.Add(user);
                        _dataAccess.Complete();

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
        public async Task<IActionResult> UserAdminDetails(string id)
        {
            if(id == null)
            {
                return NotFound();
            }

            
            _systemAdminViewModel.UserAdmin = await _dataAccess.UserAdmins.GetUserAdmin(id);

            if (_systemAdminViewModel.UserAdmin == null)
            {
                return NotFound();
            }
            _systemAdminViewModel.CurrentSystemAdmin = await _dataAccess.SystemAdmins.GetSingleSystemAdminAsync(User.Identity.Name);
            //add currentUserAdminName to persistence
            _systemAdminViewModel.CurrentSystemAdmin.CurrentUserAdminName = _systemAdminViewModel.UserAdmin.UserName;
            _dataAccess.SystemAdmins.Update(_systemAdminViewModel.CurrentSystemAdmin);
            _dataAccess.Complete();

            return View(_systemAdminViewModel);
        }

        public async Task<IActionResult> DeleteUserAdmin(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var userAdmin = await _dataAccess.UserAdmins.GetUserAdmin(id);
            _dataAccess.UserAdmins.DeleteUser(userAdmin);
            _dataAccess.Complete();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> EditUserAdmin(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            _systemAdminViewModel.UserAdmin = await _dataAccess.UserAdmins.GetUserAdmin(id);

            if(_systemAdminViewModel.UserAdmin == null)
            {
                return NotFound();
            }

            return View(_systemAdminViewModel);
        }

        public IActionResult ViewUsers()
        {
            _systemAdminViewModel.AllUsers = _dataAccess.LaundryUsers.GetAllLaundryUsers();
            return View(_systemAdminViewModel);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUserAdmin(string id,SystemAdminViewModel vm)
        {
            var userAdmin = await _dataAccess.UserAdmins.GetUserAdmin(id);

            userAdmin.Name = vm.UserAdmin.Name;
            userAdmin.Email = vm.UserAdmin.Email;
            userAdmin.UserName = vm.UserAdmin.Email;
            userAdmin.WorkAddress.StreetAddress = vm.UserAdmin.WorkAddress.StreetAddress;
            userAdmin.WorkAddress.Zipcode = vm.UserAdmin.WorkAddress.Zipcode;
            userAdmin.PhoneNumber = vm.UserAdmin.PhoneNumber;
            userAdmin.PaymentMethod = vm.UserAdmin.PaymentMethod;
            userAdmin.PaymentDueDate = vm.UserAdmin.PaymentDueDate;
            
            if (ModelState.IsValid)
            {
                var result = await _userManager.UpdateAsync(userAdmin);

                if (result.Succeeded)
                    return RedirectToAction(nameof(Index));
            }
            return View(vm.UserAdmin);
        }
    }


}