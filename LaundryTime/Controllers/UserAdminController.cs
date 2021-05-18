using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using LaundryTime.Data;
using LaundryTime.Data.Models;
using LaundryTime.Utilities;
using LaundryTime.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using JsonConverter = System.Text.Json.Serialization.JsonConverter;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace LaundryTime.Controllers
{
    public class UserAdminController : Controller //controller base??
    {
        private readonly ApplicationDbContext _context;
        private IDataAccessAction _dataAccess;
        public UserAdminViewModel _userAdminViewModel;
        protected IReportGenerator _reportGenerator;

        public UserAdminController(ApplicationDbContext context)
        {
            _context = context;
            _dataAccess = new DataAccsessAction(context);
            _userAdminViewModel = new UserAdminViewModel();
            _reportGenerator = new ReportGenerator();
        }
        
        public IActionResult Index()
        {
            if (User.HasClaim("UserAdmin", "IsUserAdmin"))
            {

                if (User.Identity != null)
                    _userAdminViewModel.CurrentUserAdmin = _dataAccess.UserAdmins.GetSingleUserAdmin(User.Identity.Name);

                return View(_userAdminViewModel);
            }
            
            return Unauthorized();
            
        }

        [HttpGet]
        public async Task<IActionResult> MyUsers(string searchname)
        {
            if (User.Identity != null && User.HasClaim("UserAdmin", "IsUserAdmin"))
            {
                var currentuser = await _dataAccess.UserAdmins.GetSingleUserAdminAsync(User.Identity.Name);
                
                _userAdminViewModel.MyUsers = currentuser.Users;


                if (!String.IsNullOrEmpty(searchname))
                {
                    _userAdminViewModel.MyUsers = (List<LaundryUser>)_userAdminViewModel.MyUsers.Where(s => s.Name.Contains(searchname));
                }

                _userAdminViewModel.MyUsers.Sort((res1, res2) => res1.Name.CompareTo(res2.Name));
                
                return View(_userAdminViewModel);
            }

            return Unauthorized();
            
        }

        [HttpGet("MyUsersReport")]
        public  async Task<IActionResult> GenerateMyUsersReport()
        {
            if (User.Identity != null && User.HasClaim("UserAdmin", "IsUserAdmin"))
            {
                var currentuser = await _dataAccess.UserAdmins.GetSingleUserAdminAsync(User.Identity.Name);

                var report = _reportGenerator.GenerateReport(currentuser.Users);

                return File(report.Content, report.Format, report.FileName);
                
            }
            return Unauthorized();
        }

        [HttpGet("MyMachinesReport")]
        public async Task<IActionResult> GenerateMyMachinesReport()
        {
            if (User.Identity != null && User.HasClaim("UserAdmin", "IsUserAdmin"))
            {
                var currentuser = await _dataAccess.UserAdmins.GetSingleUserAdminAsync(User.Identity.Name);

                var report = _reportGenerator.GenerateReport(currentuser.Machines);

                return File(report.Content, report.Format, report.FileName);
            }
            return Unauthorized();
        }

        [RequireHttps]
        public IActionResult DeleteUser(string username)
        {
            if (User.HasClaim("UserAdmin", "IsUserAdmin"))
            {
                if (username == null)
                {
                    return NotFound();
                }

                if (_userAdminViewModel.CurrentLaundryUser != null)
                {
                    if (_userAdminViewModel.CurrentLaundryUser.UserName == username || _userAdminViewModel.CurrentLaundryUser.Email == username)
                    {
                        _userAdminViewModel.CurrentLaundryUser = null;
                    }
                }

                var userToDelete = _dataAccess.LaundryUsers.GetSingleLaundryUser(username);

                _dataAccess.LaundryUsers.DeleteUser(userToDelete);
                _dataAccess.Complete();

                return RedirectToAction(nameof(MyUsers));
            }

            return Unauthorized();
        }

        [HttpGet]
        public IActionResult EditUser(string email)
        {
            if (User.HasClaim("UserAdmin", "IsUserAdmin"))
            {
                if (email == null)
                {
                    return NotFound();
                }

                _userAdminViewModel.CurrentLaundryUser = _dataAccess.LaundryUsers.GetSingleLaundryUser(email);

                if (_userAdminViewModel.CurrentLaundryUser == null)
                {
                    return NotFound();
                }

                return View(_userAdminViewModel);
            }
            
            return Unauthorized();
            
                
        }

        [HttpPost]
        public IActionResult UpdateUser(UserAdminViewModel viewModel)
        {
            if (User.HasClaim("UserAdmin", "IsUserAdmin"))
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var user = _dataAccess.LaundryUsers.GetSingleLaundryUser(viewModel.CurrentLaundryUser.UserName);

                        user.Name = viewModel.CurrentLaundryUser.Name;
                        user.PhoneNumber = viewModel.CurrentLaundryUser.PhoneNumber;
                        user.Email = viewModel.CurrentLaundryUser.Email;

                        if (user.Address == null && viewModel.CurrentLaundryUser.Address != null)
                        {
                            user.Address = new Address();
                            user.Address.StreetAddress = viewModel.CurrentLaundryUser.Address.StreetAddress;
                            user.Address.Zipcode = viewModel.CurrentLaundryUser.Address.Zipcode;
                        }

                        user.PaymentMethod = viewModel.CurrentLaundryUser.PaymentMethod;
                        user.PaymentDueDate = viewModel.CurrentLaundryUser.PaymentDueDate;
                        user.UserName = viewModel.CurrentLaundryUser.UserName;

                        _dataAccess.LaundryUsers.Update(user);
                        _dataAccess.Complete();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!_dataAccess.LaundryUsers.LaundryUserExists(viewModel.CurrentLaundryUser.Email))
                        {
                            return NotFound();
                        }
                        else
                        {
                            return RedirectToAction(nameof(MyUsers));
                        }
                    }

                    return RedirectToAction(nameof(MyUsers));
                }

                return RedirectToAction(nameof(MyUsers));
            }

            return Unauthorized();

        }

        [HttpPost]
        public IActionResult ToggleBlockUser(UserAdminViewModel viewModel)
        {
            if (User.HasClaim("UserAdmin", "IsUserAdmin"))
            {
                if (ModelState.IsValid)
                {
                    LaundryUser user;
                    try
                    {
                        user =  _dataAccess.LaundryUsers.GetSingleLaundryUser(viewModel.CurrentLaundryUser.UserName);

                        if (user.LockoutEnd == null)
                        {
                            user.LockoutEnd = new DateTimeOffset(DateTime.MaxValue);
                            user.ActiveUser = false;
                        }
                        else
                        {
                            user.LockoutEnd = null;
                            user.ActiveUser = true;
                        }

                        _dataAccess.Complete();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!_dataAccess.LaundryUsers.LaundryUserExists(viewModel.CurrentLaundryUser.Email))
                        {
                            return NotFound();
                        }

                        TempData["alertMessage"] = "Blocking/Unblocking unsuccessful";
                        return RedirectToAction("EditUser", "UserAdmin", new { email = viewModel.CurrentLaundryUser.Email });
                    }

                    TempData["alertMessage"] = "Blocking/Unblocking successful";
                    return RedirectToAction("EditUser","UserAdmin" ,new {email = user.Email });
                }
                return BadRequest();
            }
            return Unauthorized();
        }

        public IActionResult IndexMachines()
        {
            if (User.HasClaim("UserAdmin", "IsUserAdmin"))
            {
                var userAdminViewModel = new UserAdminViewModel();

                if (User.Identity != null)
                {
                    var currentUser = _dataAccess.UserAdmins.GetSingleUserAdmin(User.Identity.Name);

                    userAdminViewModel.MyMachines = currentUser.Machines;
                }

                return View(userAdminViewModel);
            }
            
            return Unauthorized();
            
        }

        [HttpGet]
        public IActionResult AddMachines()
        {
            if (User.HasClaim("UserAdmin", "IsUserAdmin"))
            {
                _userAdminViewModel.CurrentMachine = new Machine();

                return View(_userAdminViewModel);
            }

            return Unauthorized();
        }

        [HttpPost]
        public IActionResult AddMachines(UserAdminViewModel viewModel)
        {
            if (User.HasClaim("UserAdmin", "IsUserAdmin"))
            {
                if (!ModelState.IsValid)
                {
                    return NotFound();
                }

                if (User.Identity != null)
                    viewModel.CurrentMachine.UserAdmin = _dataAccess.UserAdmins.GetSingleUserAdmin(User.Identity.Name);

                _dataAccess.Machines.AddMachine(viewModel.CurrentMachine);
                _dataAccess.Complete();

                TempData["Success"] = "true";

                return RedirectToAction(nameof(IndexMachines));
            }

            return Unauthorized();
        }

        [HttpPost]
        public IActionResult DeleteMachines(string MachineToDel)
        {
            if (User.HasClaim("UserAdmin", "IsUserAdmin"))
            {
                if (!ModelState.IsValid)
                {
                    return NotFound();
                }

                _dataAccess.Machines.DelMachine(int.Parse(MachineToDel));
                _dataAccess.Complete();

                return RedirectToAction(nameof(IndexMachines));
            }

            return Unauthorized();

        }
    }
}
