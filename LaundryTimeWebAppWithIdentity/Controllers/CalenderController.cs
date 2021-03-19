using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryTimeWebAppWithIdentity.Controllers
{
    public class CalenderController : Controller
    {
        public IActionResult Index()
        {
            return View();
            //Palle
        }
        public IActionResult Calender()
        {
            return View();
        }
    }
}
