using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Projekt4.DataAccess.Repository.IRepository;
using Projekt4.Models;

namespace Projekt4.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class MaskineController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;

		public MaskineController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Upsert(int? id)
		{
			Maskine maskine = new Maskine();
			if (id == null)
			{
				//this is for create
				return View(maskine);
			}
			//this is for edit
			maskine = _unitOfWork.Maskine.Get(id.GetValueOrDefault());
			if (maskine == null)
			{
				return NotFound();
			}
			return View(maskine);

		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Upsert(Maskine maskine)
		{
			if (ModelState.IsValid)
			{
				if (maskine.Id == 0)
				{
					_unitOfWork.Maskine.Add(maskine);
					_unitOfWork.Save();
				}
				else
				{
					_unitOfWork.Maskine.Update(maskine);
				}
				_unitOfWork.Save();
				return RedirectToAction(nameof(Index));
			}
			return View(maskine);
		}


		#region API Calls

		[HttpGet]
		public IActionResult GetAll()
		{
			var allObj = _unitOfWork.Maskine.GetAll();
			return Json(new { data = allObj });
		}

		[HttpDelete]
		public IActionResult Delete(int id)
		{
			var objFromDb = _unitOfWork.Maskine.Get(id);
			if (objFromDb == null)
			{
				return Json(new { success = false, message = "Error while deleting" });
			}

			_unitOfWork.Maskine.Remove(objFromDb);
			_unitOfWork.Save();
			return Json(new { success = true, message = "Delete Successful" });
		}

		#endregion
	}
}
