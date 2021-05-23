using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LaundryTime.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

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
        [Authorize]
        public IActionResult Index()
        {

            if (User.HasClaim("SystemAdmin", "IsSystemAdmin"))
            {
                return RedirectToAction(nameof(Index), "SystemAdmin");
            }
            if (User.HasClaim("UserAdmin", "IsUserAdmin"))
            {
                return RedirectToAction(nameof(Index), "UserAdmin");
            }
            if (User.HasClaim("LaundryUser", "IsLaundryUser"))
            {
                return RedirectToAction(nameof(Index), "LaundryUser");
            }

            else
            {
                TempData["Security Check"] = "failed";
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult ContactUs()
        {
            return View();
        }

    }
}
