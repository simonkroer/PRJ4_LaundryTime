﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LaundryTimeWebAppWithIdentity.Models;
using System.Text;
using LaundryTimeWebAppWithIdentity.Models.Calender;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LaundryTimeWebAppWithIdentity.Controllers
{
    public class CalenderController : Controller
    {
        public IActionResult Calender()
        {
            //SeedDatabase temp = new SeedDatabase();
            //temp.SeedData();
            return View();
        }

    }

    //public class Clock
    //{
    //    Timer t = new Timer();
    //}
}