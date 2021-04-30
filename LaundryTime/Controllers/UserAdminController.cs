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
            return View();
        }

        //[Authorize("IsUserAdmin")]
        public IActionResult MyUsers()
        {
            if (User.Identity != null)
            {
                var currentuser = _dataAccess.UserAdmins
	                .GetSingleUserAdmin(User.Identity.Name);

                _userAdminViewModel.MyUsers = currentuser.Users;
            }

            return View(_userAdminViewModel);
        }

        //[Authorize("IsUserAdmin")]
        public IActionResult AddUsers()
        {
            return View(_userAdminViewModel);
        }

        //[Authorize("IsUserAdmin")]
        public IActionResult DeleteUser()
        {
            //Delete the chosen user HERE

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

        //Virker ikke endnu. Der kommer blot en nyt laundryUser med som er tom. 
        public IActionResult UpdateUser([Bind(Prefix = nameof(UserAdminViewModel.CurrentLaundryUser))] UserAdminViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var name = viewModel.CurrentLaundryUser.Name; //Just for testing
                    _dataAccess.LaundryUsers.Update(viewModel.CurrentLaundryUser);
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
                        throw;
                    }
                }

                return RedirectToAction(nameof(EditUser));
            }

            return RedirectToAction(nameof(MyUsers));
        }




        //[Authorize("IsUserAdmin")]
        public IActionResult IndexMachines()
        {
            var userAdminViewModel = new UserAdminViewModel();

            userAdminViewModel.MyMachines = _dataAccess.Machines.GetAllMachines();

            return View(userAdminViewModel);
        }

        //[Authorize("IsUserAdmin")]
        public IActionResult AddMachines()
        {
            var userAdminViewModel = new UserAdminViewModel();

            userAdminViewModel.MyMachines = _dataAccess.Machines.GetAllMachines();

            return View(userAdminViewModel);
        }

        //[Authorize("IsUserAdmin")]
        public IActionResult DeleteMachines()
        {
            var userAdminViewModel = new UserAdminViewModel();

            userAdminViewModel.MyMachines = _dataAccess.Machines.GetAllMachines();

            return View(userAdminViewModel);
        }
    }
}
