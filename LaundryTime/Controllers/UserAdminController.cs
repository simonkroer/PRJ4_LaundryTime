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
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;

namespace LaundryTime.Controllers
{
    public class UserAdminController : Controller
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

        //[Authorize("IsUserAdmin")]
        public IActionResult Index()
        {
            _userAdminViewModel.CurrentUserAdmin = _dataAccess.UserAdmins.GetSingleUserAdmin(User.Identity.Name);
            return View(_userAdminViewModel);
        }

        //[Authorize("IsUserAdmin")]
        public IActionResult MyUsers()
        {
            if (User.Identity != null)
            {
                var currentuser = _dataAccess.UserAdmins.GetSingleUserAdmin(User.Identity.Name);

                _userAdminViewModel.MyUsers = currentuser.Users;
            }

            return View(_userAdminViewModel);
        }


        //[Authorize("IsUserAdmin")]
        [HttpPost]
        public IActionResult DeleteUser(string username)
        {
            if (username == null)
            {
                return NotFound();
            }
            
            var userToDelete = _dataAccess.LaundryUsers.GetSingleLaundryUser(username);

            _dataAccess.LaundryUsers.DeleteUser(userToDelete);
            _dataAccess.Complete();

            return RedirectToAction(nameof(MyUsers));
        }

        [HttpGet]
        public IActionResult EditUser(string email)
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

        [HttpPost]
        //[Authorize("IsUserAdmin")]
        public IActionResult UpdateUser(UserAdminViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = _dataAccess.LaundryUsers.GetSingleLaundryUser(viewModel.CurrentLaundryUser.UserName);

                    user.Name = viewModel.CurrentLaundryUser.Name;
                    user.PhoneNumber = viewModel.CurrentLaundryUser.PhoneNumber;
                    user.Email = viewModel.CurrentLaundryUser.Email;

                    if (user.Address == null && viewModel.CurrentLaundryUser.Address!=null)
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




        //[Authorize("IsUserAdmin")]
        public IActionResult IndexMachines()
        {
            var userAdminViewModel = new UserAdminViewModel();

            var currentUser = _dataAccess.UserAdmins.GetSingleUserAdmin(User.Identity.Name);

            userAdminViewModel.MyMachines = currentUser.Machines;

            return View(userAdminViewModel);
        }

        //[Authorize("IsUserAdmin")]
        [HttpGet]
        public IActionResult AddMachines()
        {
            _userAdminViewModel.CurrentMachine = new Machine();

            return View(_userAdminViewModel);
        }

        //[Authorize("IsUserAdmin")]
        [HttpPost]
        public IActionResult AddMachines(UserAdminViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            viewModel.CurrentMachine.UserAdmin = _dataAccess.UserAdmins.GetSingleUserAdmin(User.Identity.Name);

            _dataAccess.Machines.AddMachine(viewModel.CurrentMachine);
            _dataAccess.Complete();

            TempData["Success"] = "true";

            return RedirectToAction(nameof(IndexMachines));
        }

        //[Authorize("IsUserAdmin")]
        public IActionResult DeleteMachines()
        {
            _userAdminViewModel.MyMachines = _dataAccess.Machines.GetAllMachines();

            return View(_userAdminViewModel);
        }
    }
}
