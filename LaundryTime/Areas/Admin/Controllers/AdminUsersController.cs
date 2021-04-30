using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaundryTime.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace LaundryTime.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class AdminUserController : Controller
	{
		private readonly UserManager<UserAdmin> userManager;

		public AdminUserController(UserManager<UserAdmin> userManager)
		{
			this.userManager = userManager;
		}


		public IActionResult Index()
		{
			return View();
		}
	}
}
