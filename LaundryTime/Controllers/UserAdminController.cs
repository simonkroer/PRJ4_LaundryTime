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
                var currentuser = _dataAccess.UserAdmins.GetSingleUserAdmin(User.Identity.Name);

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

        public async Task<IActionResult> UpdateUser(string username, [Bind("Name,Phone,Email,Address,Paymentmethod,GuestType,HotelRoom")] LaundryUser laundryUser)
        {
            if (username != laundryUser.Email)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dataAccess.LaundryUsers.Update(laundryUser);
                    _dataAccess.Complete();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_dataAccess.LaundryUsers.LaundryUserExists(laundryUser.Email))
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

            return RedirectToAction(nameof(EditUser));
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
