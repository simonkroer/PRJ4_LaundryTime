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

        public async Task<IActionResult> AvailableBookings(DateViewModel obj)
        {
            //obj.Datedata = DateTime.Parse("22-04-2021");
            var bookingList = await _dataAccess.BookingList.GetAllAvalableBookings(obj.Datedata);
            if (bookingList.Count == 0)
            {
                BookingSeeder bs = new BookingSeeder();
                var datemodel = bs.CreateDateModel(_context, obj.Datedata.Date.ToString());
                bs.CreateNewBookList(_context, datemodel);
                
            }
            //bookingList = await _dataAccess.BookingList.GetAllAvalableBookings(obj.Datedata);

            List<BookingListViewModel> modelList = new List<BookingListViewModel>();

            foreach (var booking in bookingList)
            {
                if (booking.Status)
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
            modelList.Sort((res1, res2) => res1.MachineName.CompareTo(res2.MachineName));
            return View(modelList);
        }

        public async Task<IActionResult> Book(long? id)
        {
            var bookingOrder = await _dataAccess.BookingList.SingleBook(id);
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
                //_context.ReservedListModels.Add(reservedBookings);
                _dataAccess.ReservedList.AddSingleReservation(reservedBookings);
                bookingOrder.Status = false;
                var LUser = User.Identity.Name;
                var tempUser = _dataAccess.LaundryUsers.GetSingleLaundryUser(LUser);
                var laundryLog = new LaundryLog()
                {
                    LaundryUser = tempUser,
                    LogDate = DateTime.Now,
                    LogInfo = $"Booked machine {reservedBookings.Machine.MachineId} of the type {reservedBookings.Machine.Type} for {reservedBookings.Date} at {reservedBookings.Time}"
                };
                _dataAccess.LaundryLogs.AddLaundryLog(laundryLog);
                _dataAccess.Complete();
            }

            var BookingList = await _dataAccess.BookingList.GetBookingList();
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
        public async Task<IActionResult> Unbook(long? id)
        {
            var unBookOrder = await _dataAccess.ReservedList.GetUnBookOrder(id);
            var bookingOrder = await _dataAccess.BookingList.GetBookingListOrder(unBookOrder.OldId);

            if (bookingOrder == null || unBookOrder == null)
            {
                return NotFound();
            }
            else
            {
                bookingOrder.Status = true;
                _dataAccess.ReservedList.RemoveBooking(unBookOrder);
                /*Log Entry*/
                var LUser = User.Identity.Name;
                var tempUserUn = _dataAccess.LaundryUsers.GetSingleLaundryUser(LUser);
                var laundryLogUn = new LaundryLog();
                laundryLogUn.LaundryUser = tempUserUn;
                laundryLogUn.LogDate = DateTime.Now;
                laundryLogUn.LogInfo = ($"Unbooked machine {unBookOrder.Machine.MachineId} that was reserved at {unBookOrder.Date} at {unBookOrder.Time}");
                _dataAccess.LaundryLogs.AddLaundryLog(laundryLogUn);
                _dataAccess.Complete();
            }

            var ReservedBookingList = await _dataAccess.ReservedList.GetReservedBookingList();
            List<BookingListViewModel> modelList = new List<BookingListViewModel>();

            foreach (var booking in ReservedBookingList)
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
                    _dataAccess.Complete();
                }
            }

            return View("UsersBookings", modelList);
        }
        public async Task<IActionResult> UsersBookings()
        {
            var BookingList = await _dataAccess.ReservedList.GetReservedBookingList();
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
            modelList.Sort((res1, res2) => res1.Date.CompareTo(res2.Date));
            return View(modelList);
        }
        
        public IActionResult SendMessageToUserAdmin(string message)
        {
            if (message == null)
            {
                return View();
            }
            else
            {
                var msg = new MessageToUserAdmin();
                var LUser = User.Identity.Name;
                var tempUser = _dataAccess.LaundryUsers.GetSingleLaundryUser(LUser);
                msg.LaundryUser = tempUser;
                msg.SendDate = DateTime.Now;
                msg.MessageInfo = message;
                if (!ModelState.IsValid)
                {
                    return NotFound();
                }
                _dataAccess.MessageList.SendMessage(msg);
                _dataAccess.Complete();

                return RedirectToAction(nameof(Index));
            }
        }
    }
}
