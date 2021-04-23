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

namespace LaundryTime.Controllers
{
    public class UserAdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IDataAccessAction _dataAccess;
        private UserAdminViewModel userAdminViewModel;

        public UserAdminController(ApplicationDbContext context)
        {
            _context = context;
            _dataAccess = new DataAccsessAction(context);
            userAdminViewModel = new UserAdminViewModel();
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
