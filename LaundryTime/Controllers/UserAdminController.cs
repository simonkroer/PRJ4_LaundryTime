using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaundryTime.Data;
using LaundryTime.Data.Models;
using LaundryTime.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace LaundryTime.Controllers
{
    public class UserAdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IDataAccessAction _dataAccess;

        UserAdminController(ApplicationDbContext context)
        {
            _context = context;
            _dataAccess = new DataAccsessAction(context);
        }

        [Authorize("IsUserAdmin")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize("IsUserAdmin")]
        public IActionResult MyUsers()
        {
            var userAdminViewModel = new UserAdminViewModel();

            userAdminViewModel.MyUsers = _dataAccess.UserAdmins.GetAllMyUsers();

            return View(userAdminViewModel);
        }

        [Authorize("IsUserAdmin")]
        public IActionResult MyMachines()
        {
            var userAdminViewModel = new UserAdminViewModel();

            userAdminViewModel.MyMachines = _dataAccess.Machines.GetAllMachines();

            return View(userAdminViewModel);
        }



    }
}
