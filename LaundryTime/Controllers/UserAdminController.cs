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
        

        public UserAdminController(ApplicationDbContext context)
        {
            _context = context;
            _dataAccess = new DataAccsessAction(context);
        }

        //[Authorize("IsUserAdmin")]
        public IActionResult Index()
        {
            return View();
        }

        //[Authorize("IsUserAdmin")]
        public IActionResult MyUsers()
        {
            var userAdminViewModel = new UserAdminViewModel();

            if (User.Identity != null)
            {
                var currentuser = _dataAccess.UserAdmins.GetSingleUserAdmin(User.Identity.Name);

                userAdminViewModel.MyUsers = currentuser.Users;
            }

            return View(userAdminViewModel);
        }

        //[Authorize("IsUserAdmin")]
        public IActionResult AddUsers(UserAdminViewModel userAdminViewModel)
        {
            return View(userAdminViewModel);
        }

        //[Authorize("IsUserAdmin")]
        public IActionResult DeleteUser()
        {
            //Delete the chosen user HERE

            return RedirectToAction(nameof(MyUsers));
        }

        public async Task<IActionResult> EditUser(string? username)
        {
            if (username == null)
            {
                return NotFound();
            }

            var laundryuser = _dataAccess.LaundryUsers.GetSingleLaundryUser(username);

            if (laundryuser == null)
            {
                return NotFound();
            }

            return View(laundryuser);
        }

        public async Task<IActionResult> EditUser(string username, [Bind("GuestID,FirstName,LastName,IsCheckedIn,HasEatenBreakfast,GuestType,HotelRoom")] LaundryUser user)
        {
            if (username != user.UserName)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    _dataAccess.Complete();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_dataAccess.LaundryUsers.LaundryUserExists(user.Email))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(MyUsers));
            }

            return View(user);
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
