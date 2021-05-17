using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using LaundryTime.Data;
using LaundryTime.Data.Models;
using LaundryTime.Data.Models.Booking;
using LaundryTime.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

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
            DateViewModel dp = new DateViewModel()
            {
                Datedata = DateTime.Now.Date
            };
            return View(dp);
        }

        public IActionResult AvailableBookings(DateViewModel obj)
        {
            //obj.Datedata = DateTime.Parse("22-04-2021");
            var bookingList = _context.BookingListModels.Where(b => b.Date.Date == obj.Datedata.Date).Include(b => b.Machine);
            if (bookingList.Any() == false)
            {
                BookingSeeder bs = new BookingSeeder();
                var datemodel = bs.CreateDateModel(_context, obj.Datedata.Date.ToString());
                bs.CreateNewBookList(_context, datemodel);
                
            }
            bookingList = _context.BookingListModels.Where(b => b.Date.Date == obj.Datedata.Date).Include(b => b.Machine);
            List<BookingListViewModel> modelList = new List<BookingListViewModel>();

            foreach (var booking in bookingList)
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
            var bookingOrder = _context.BookingListModels.Include(b => b.Machine).FirstOrDefault(b => b.Id == id);
            if (bookingOrder == null)
            {
                return NotFound();
            }
            else
            {
                var reservedBookings = new ReservedListModel()
                {
                    Date = bookingOrder.Date,
                    Machine = bookingOrder.Machine,
                    Time = bookingOrder.Time,
                    OldId = bookingOrder.Id,
                    Name = User.Identity.Name
                };
                _context.ReservedListModels.Add(reservedBookings);
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

            DateViewModel dvm = new DateViewModel()
            {
                Datedata = bookingOrder.Date
            };
            
            return RedirectToAction("AvailableBookings", dvm);
        }
        public IActionResult Unbook(long? id)
        {
            var unBookOrder = _context.ReservedListModels.FirstOrDefault(r => r.Id == id);
            var bookingOrder = _context.BookingListModels.FirstOrDefault(b => b.Id == unBookOrder.OldId);
            if (bookingOrder == null || unBookOrder == null)
            {
                return NotFound();
            }
            else
            {
                bookingOrder.Status = true;
                _context.Remove(unBookOrder);
                _context.SaveChanges();
            }

            var BookingList = _context.ReservedListModels.Include(r => r.Machine);
            List<BookingListViewModel> modelList = new List<BookingListViewModel>();

            foreach (var booking in BookingList)
            {
                if (booking.Name == User.Identity.Name)
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
        public IActionResult UsersBookings()
        {
            var BookingList = _context.ReservedListModels.Include(r => r.Machine);
            List<BookingListViewModel> modelList = new List<BookingListViewModel>();

            foreach (var booking in BookingList)
            {
                if (booking.Name == User.Identity.Name)
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
    }
}
