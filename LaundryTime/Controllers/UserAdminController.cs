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
using LaundryTime.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
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

        public UserAdminController(ApplicationDbContext context)
        {
            _context = context;
            _dataAccess = new DataAccsessAction(context);
            _userAdminViewModel = new UserAdminViewModel();
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
        public async Task<IActionResult> MyUsers()
        {
            if (User.Identity != null && User.HasClaim("UserAdmin", "IsUserAdmin"))
            {
                var currentuser = await _dataAccess.UserAdmins.GetSingleUserAdminAsync(User.Identity.Name);

                _userAdminViewModel.MyUsers = currentuser.Users;

                return View(_userAdminViewModel);
            }
            
            return Unauthorized();
            
        }

        [HttpGet("MyUsersReport")]
        public  async Task<IActionResult> GenerateMyUsersReport()
        {
            if (User.Identity != null && User.HasClaim("UserAdmin", "IsUserAdmin"))
            {
                var builder = new StringBuilder();
                var currentuser = await _dataAccess.UserAdmins.GetSingleUserAdminAsync(User.Identity.Name);

                _userAdminViewModel.MyUsers = currentuser.Users;

                foreach (var user in _userAdminViewModel.MyUsers) // Alternative: Build json object manually
                {
                    var builder2 = new StringBuilder();

                    foreach (var log in user.LaundryHistory)
                    {
                        log.LaundryUser = null;
                        builder2.Append(log);
                    }

                    builder.Append(JsonConvert.SerializeObject(user));
                    builder.Append(JsonConvert.SerializeObject(builder2));
                }

                
                string filename = "MyUsersReport.json";

                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(builder.ToString());
                var content = new MemoryStream(bytes);

                return File(content, "text/json", filename);

            }

            return Unauthorized();

        }

        [HttpGet("MyMachinesReport")]
        public async Task<IActionResult> GenerateMyMachinesReport()
        {
            if (User.Identity != null && User.HasClaim("UserAdmin", "IsUserAdmin"))
            {
                var builder = new StringBuilder();
                var currentuser = await _dataAccess.UserAdmins.GetSingleUserAdminAsync(User.Identity.Name);

                _userAdminViewModel.MyMachines = currentuser.Machines;

                foreach (var machine in _userAdminViewModel.MyMachines) // Alternative: Build json object manually
                {
                    machine.UserAdmin = null;
                    builder.Append(JsonConvert.SerializeObject(machine));
                }

                string filename = "MyMachinesReport.json";

                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(builder.ToString());
                var content = new MemoryStream(bytes);

                return File(content, "text/json", filename);
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
