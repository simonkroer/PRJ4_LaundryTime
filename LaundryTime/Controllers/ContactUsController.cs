﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryTime.Controllers
{
    public class ContactUsController : Controller
    {
        [RequireHttps]
        public IActionResult ContactUs()
        {
            return View();
        }
    }
}
