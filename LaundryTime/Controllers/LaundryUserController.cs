using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaundryTime.Data;
using LaundryTime.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace LaundryTime.Controllers
{
    public class LaundryUserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IDataAccessAction _dataAccess;
        
        public LaundryUserController(ApplicationDbContext context)
        {
            _context = context;
            _dataAccess = new DataAccsessAction(context);
        }

        public IActionResult Index()
        {
            var BookingList = _context.BookingListModels.Include(b => b.Machine);
            List<BookingListViewModel> modelList = new List<BookingListViewModel>();

            foreach (var booking in BookingList)
            {
                if (booking.Status == true)
                {
                    BookingListViewModel model = new BookingListViewModel();
                    model.BookingID = booking.Id;
                    model.Date = booking.Date;
                    model.MachineName = booking.Machine.MachineId;
                    model.MachineType = booking.Machine.Type;
                    model.Time = booking.Time;

                    modelList.Add(model);
                }
                
            }
            
            return View(modelList);
        }

        public IActionResult Book(long? id)
        {
            var bookingOrder = _context.BookingListModels.FirstOrDefault(b => b.Id == id);
            if (bookingOrder == null)
            {
                return NotFound();
            }
            else
            {
                bookingOrder.Status = false;
                _context.SaveChanges();
            }

            var BookingList = _context.BookingListModels.Include(b => b.Machine);
            List<BookingListViewModel> modelList = new List<BookingListViewModel>();

            foreach (var booking in BookingList)
            {
                if (booking.Status == true)
                {
                    BookingListViewModel model = new BookingListViewModel();
                    model.BookingID = booking.Id;
                    model.Date = booking.Date;
                    model.MachineName = booking.Machine.MachineId;
                    model.MachineType = booking.Machine.Type;
                    model.Time = booking.Time;
                    
                    modelList.Add(model);
                }

            }

            return View("Index",modelList);
        }
        public IActionResult Unbook(long? id)
        {
            var bookingOrder = _context.BookingListModels.FirstOrDefault(b => b.Id == id);
            if (bookingOrder == null)
            {
                return NotFound();
            }
            else
            {
                bookingOrder.Status = true;
                _context.SaveChanges();
            }

            var BookingList = _context.BookingListModels.Include(b => b.Machine);
            List<BookingListViewModel> modelList = new List<BookingListViewModel>();

            foreach (var booking in BookingList)
            {
                if (booking.Status == false)
                {
                    BookingListViewModel model = new BookingListViewModel();
                    model.BookingID = booking.Id;
                    model.Date = booking.Date;
                    model.MachineName = booking.Machine.MachineId;
                    model.MachineType = booking.Machine.Type;
                    model.Time = booking.Time;

                    modelList.Add(model);
                }

            }

            return View("UsersBookings", modelList);
        }
    }
}
