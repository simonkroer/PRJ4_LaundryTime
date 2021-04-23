using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaundryTime.Data;
using LaundryTime.Data.Models;

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
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MyUsersView()
        {
            return View();
        }
    }
}
