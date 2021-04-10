using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Projekt4.DataAccess.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Projekt4.DataAccess.Repository.IRepository;
using Projekt4.Models;

namespace Projekt4.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class UserController : Controller
	{
		private readonly ApplicationDbContext _db;

		public UserController(ApplicationDbContext db)
		{
			_db = db;
		}

		public IActionResult Index()
		{
			return View();
		}


		#region API Calls

		[HttpGet]
		public IActionResult GetAll()
		{
			var userList = _db.ApplicationUsers.ToList();
			var userRole = _db.UserRoles.ToList();
			var roles = _db.Roles.ToList();
			foreach (var user in userList)
			{
				var roleId = userRole.FirstOrDefault(u => u.UserId == user.Id).RoleId;
				user.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;
					
			}
			return Json(new { data = userList });
		}

		#endregion
	}
}
