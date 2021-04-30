using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryTime.Controllers
{
    public class LaundryUserController : Controller
    {
        public IActionResult Book()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
