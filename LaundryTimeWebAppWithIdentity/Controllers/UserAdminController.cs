using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaundryTimeWebAppWithIdentity.Data;
using Microsoft.AspNetCore.Mvc;

namespace LaundryTimeWebAppWithIdentity.Controllers
{ public class UserAdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserAdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> MyUsers()
        {
            return View(await _context._UserAdmins.ToListAsync());
        }

    }
}
