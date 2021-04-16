using LaundryTime.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LaundryTime.Data.Models;
using Microsoft.AspNetCore.Authorization;

namespace LaundryTime.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return RedirectToPage("/Account/Login");
        }

        public IActionResult CheckClaim()
        {
            if (User.HasClaim("User", "IsUser"))
            {
                return RedirectToAction(nameof(Index), "User");
            }

            if (User.HasClaim("UserAdmin", "IsUserAdmin"))
            {
                return RedirectToAction(nameof(Index), "UserAdmin");
            }

            if (User.HasClaim("SystemAdmin", "IsSystemAdmin"))
            {
                return RedirectToAction(nameof(Index), "SystemAdmin");
            }

            else
            {
                TempData["Security Check"] = "failed";
                return RedirectToAction(nameof(Index));
            }
        }

        //public IActionResult

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
