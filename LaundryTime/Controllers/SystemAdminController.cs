using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaundryTime.Data;
using LaundryTime.ViewModels;

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
            return View();
        }
    }
}
