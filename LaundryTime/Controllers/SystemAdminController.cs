using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaundryTime.Data;
using LaundryTime.Data.Models;
using LaundryTime.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace LaundryTime.Controllers
{

	public class SystemAdminController : Controller
	{
		private readonly ApplicationDbContext _context;
		private IDataAccessAction _dataAccess;
		public SystemAdminViewModel _systemAdminViewModel;

		public SystemAdminController(ApplicationDbContext context)
		{
			_context = context;
			_dataAccess = new DataAccsessAction(context);
			_systemAdminViewModel = new SystemAdminViewModel();
		}



		public IActionResult Index()
		{
			if (User.HasClaim("SystemAdmin", "IsSystemAdmin"))
			{
				_systemAdminViewModel.AllUserAdmins = _dataAccess.UserAdmins.GetAllUserAdmins();

				return View(_systemAdminViewModel);
			}
			return Unauthorized();
		}

		//// Post /account/Register
		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public async Task<IActionResult> Register(UserAdmin userAdmin)
		//{

		//	if (ModelState.IsValid)
		//	{
		//		var appUser = new UserAdmin()
		//		{
		//			Name = userAdmin.Name,
		//			PaymentMethod = userAdmin.PaymentMethod,
		//		};

		//		_dataAccess.UserAdmins.AddUserAdmin(appUser);


		//	}
		//	return View(user);
		//}
	}
}