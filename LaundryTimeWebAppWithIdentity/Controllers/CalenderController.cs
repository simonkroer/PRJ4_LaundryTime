using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LaundryTimeWebAppWithIdentity.Controllers
{
    public class CalenderController : Controller
    {
        public IActionResult Calender()
        {
            return View();
        }
    }

    //public class Clock
    //{
    //    Timer t = new Timer();
    //}
}
