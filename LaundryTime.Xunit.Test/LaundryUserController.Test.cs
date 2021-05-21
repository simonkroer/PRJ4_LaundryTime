using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using LaundryTime.Controllers;
using LaundryTime.Data;
using LaundryTime.Data.Models;
using LaundryTime.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Twilio.TwiML.Voice;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace LaundryTime.Xunit.Test
{
    public class LaundryUserControllerTest
    {
        protected ApplicationDbContext _context { get; set; }
        protected LaundryUserController _uut;

        public LaundryUserControllerTest()
        {
            _context = new ApplicationDbContext(
                new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(CreateInMemoryDatabase()).Options);

            Seed();

            _uut = new LaundryUserController(_context);

        }


        #region Index
        [Fact]
        public void Index_User_ExpectedIActionResult()
        {
            _uut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim("LaundryUser", "IsLaundryUser")
                    }))
                }
            };

            var res = _uut.Index();
           
            Assert.IsType<ViewResult>(res);
            Dispose();
        }

        [Fact]
        public void Index_User_Correct_Expected_ViewModel()
        {
            _uut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim("LaundryUser", "IsLaundryUser")
                    }))
                }
            };

            var res = _uut.Index() as ViewResult;
            var modelData = res.Model;

            Assert.IsType<DateViewModel>(modelData);


            Dispose();
        }

        [Fact]
        public void Index_AuthorizedUser_Expected_ViewNameCorrect_ModelNotNull()
        {
            _uut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim("LaundryUser", "IsLaundryUser")
                    }))
                }
            };
            
            var res = _uut.Index() as ViewResult;
            var viewname = res.ViewName;
            var temp = res.Model;

            Assert.True(string.IsNullOrEmpty(viewname) || viewname == "Index");
            Assert.NotNull(temp);

            Dispose();
        }



        #endregion


        #region AvailableBookings

        [Fact]
        public void AvailableBookings_Expected_View()
        {
            _uut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim("LaundryUser", "IsLaundryUser")
                    }))
                }
            };
            var temp = Substitute.For<DateViewModel>();
            temp.Datedata = DateTime.Now;
            var res = _uut.AvailableBookings(temp);

            Assert.IsType<Task<IActionResult>>(res);
            Dispose();

            

        }
        [Fact]
        public async Task AvailableBookings_AuthorizedUser_ExpectedResult()
        {
            _uut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim("LaundryUser", "IsLaundryUser")
                    }))
                }
            };
            var temp = Substitute.For<DateViewModel>();
            temp.Datedata = DateTime.Now;
            var res = await _uut.AvailableBookings(temp);
            
            Assert.IsType<ViewResult>(res);
            Assert.Equal((int)HttpStatusCode.OK, _uut.ControllerContext.HttpContext.Response.StatusCode);

            Dispose();

        }


        #endregion

        #region Book
        [Fact]
        public async Task Book_AuthorizedUser_ExpectedHTTPdResult()
        {
            _uut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim("LaundryUser", "IsLaundryUser")
                    }))
                }
            };
            BookingSeeder bs = new BookingSeeder();
            bs.CreateNewBookList(_context, bs.CreateDateModel(_context, "2021-05-21"));

            long Id = 1; // første object i bookingList
            var booking = await _context.BookingListModels.Include(b => b.Machine).FirstOrDefaultAsync(b => b.Id == Id);
            
            var res = await _uut.Book(Id);

            Assert.Equal((int)HttpStatusCode.OK, _uut.ControllerContext.HttpContext.Response.StatusCode);

            Dispose();
        }

        [Fact]
        public async Task Book_AuthorizedUser_ExpectedDatabaseResult()
        {
            _uut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim("LaundryUser", "IsLaundryUser")
                    }))
                }
            };
            BookingSeeder bs = new BookingSeeder();
            bs.CreateNewBookList(_context,bs.CreateDateModel(_context, "2021-05-21"));
            int Id = 1; // første object i bookingList
            var booking = await _context.BookingListModels.Include(b => b.Machine).FirstOrDefaultAsync(b => b.Id == Id);
            await _uut.Book(Id);
            var reserved = await _context.ReservedListModels.Include(b => b.Machine)
                .FirstOrDefaultAsync(r => r.OldId == Id);
            
            Assert.Equal(reserved.OldId, Id);
            Assert.Equal(booking.MachineId, reserved.MachineId);
            Assert.Equal(booking.Time, reserved.Time);


            Dispose();
        }

        [Fact]
        public async Task Book_AuthorizedUser_BookNotFound_ExpectedResult()
        {
            _uut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim("LaundryUser", "IsLaundryUser")
                    }))
                }
            };
            BookingSeeder bs = new BookingSeeder();
            bs.CreateNewBookList(_context, bs.CreateDateModel(_context, "2021-05-21"));
            var bookinglist = _context.BookingListModels.ToList();
            
            
            int Id = bookinglist.Count +1; // Sikre at skaffe et id et større end antal bookings i databasen

            var res = _uut.Book(Id); 
            var tmp = res.Result;


            Assert.IsType<NotFoundResult>(tmp);


            Dispose();
        }


        #endregion

        #region Unbook

        [Fact]
        public async Task UnBook_AuthorizedUser_ExpectedHTTPdResult()
        {
            _uut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim("LaundryUser", "IsLaundryUser")
                    }))
                }
            };

            long Id = 1; // første object i bookingList
            BookingSeeder bs = new BookingSeeder();
            bs.CreateNewBookList(_context, bs.CreateDateModel(_context, "2021-05-21"));
            var booking = await _context.BookingListModels.Include(b => b.Machine).FirstOrDefaultAsync(b => b.Id == Id);

            var res = await _uut.Book(Id);

            Assert.Equal((int)HttpStatusCode.OK, _uut.ControllerContext.HttpContext.Response.StatusCode);

            Dispose();
        }

        [Fact]
        public async Task UnBook_AuthorizedUser_ExpectedDatabaseResult()
        {
            _uut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim("LaundryUser", "IsLaundryUser")
                    }))
                }
            };
            BookingSeeder bs = new BookingSeeder();
            bs.CreateNewBookList(_context, bs.CreateDateModel(_context, "2021-05-21"));
            int Id = 1; // første object i bookingList
            int Id2 = 2;
            var booking = await _context.BookingListModels.Include(b => b.Machine).FirstOrDefaultAsync(b => b.Id == Id);
            var booking2 = await _context.BookingListModels.Include(b => b.Machine).FirstOrDefaultAsync(b => b.Id == Id2);
            await _uut.Book(Id);
            await _uut.Book(Id2);
            await _uut.Unbook(Id);
            var reserved = await _context.ReservedListModels.Include(b => b.Machine)
                .FirstOrDefaultAsync(r => r.OldId == Id);

            Assert.Null(reserved);
            Dispose();
        }

        [Fact]
        public async Task UnBook_AuthorizedUser_BookNotFound_ExpectedResult()
        {
            _uut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim("LaundryUser", "IsLaundryUser")
                    }))
                }
            };
            BookingSeeder bs = new BookingSeeder();
            bs.CreateNewBookList(_context, bs.CreateDateModel(_context, "2021-05-21"));
            int Id = 1; // første object i bookingList
            int Id2 = 2;
            long Id3 = 3;
            var booking = await _context.BookingListModels.Include(b => b.Machine).FirstOrDefaultAsync(b => b.Id == Id);
            var booking2 = await _context.BookingListModels.Include(b => b.Machine).FirstOrDefaultAsync(b => b.Id == Id2);
            await _uut.Book(Id);
            await _uut.Book(Id2);
            var res = _uut.Unbook(Id3);
            var tmp = res.Result;


            Assert.IsType<NotFoundResult>(tmp);


            Dispose();
        }

        #endregion


        #region UsersBookings
        [Fact]
        public void UsersBookings_Expected_View()
        {
            _uut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim("LaundryUser", "IsLaundryUser")
                    }))
                }
            };

            
            var res = _uut.UsersBookings();

            Assert.IsType<Task<IActionResult>>(res);
            Dispose();
        }

        [Fact]
        public async Task UsersBookings_AuthorizedUser_ExpectedResult()
        {
            _uut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim("LaundryUser", "IsLaundryUser")
                    }))
                }
            };

            var taskRes = await _uut.UsersBookings();

            var res = taskRes as ViewResult;
            var modelData = res.Model;

            Assert.IsType<List<BookingListViewModel>>(modelData);


            Dispose();

        }


        #endregion

        #region Setup Methods
        static DbConnection CreateInMemoryDatabase()
        {
            var connection = new SqliteConnection("Filename=:memory:");//Fake db
            connection.Open();
            return connection;
        }

        public void Seed()
        {
            _context.Database.EnsureCreated();

            var user1 = new LaundryUser()
            {
                Name = "testerlaundryuser",
                PaymentMethod = "cash",
                Address = new Address() { Country = "Denmark", StreetAddress = "Testvej 1", Zipcode = "8700" },
                ActiveUser = true,
                FinancialBalance = 1200,
                PaymentDueDate = new DateTime(2021 - 10 - 08),
                UserName = "testusername1",
                Email = "test1@user.dk"
            };
            var user2 = new LaundryUser()
            {
                Name = "testerlaundryuser2",
                PaymentMethod = "mobilepay",
                Address = new Address() { Country = "Denmark", StreetAddress = "Testvej 1", Zipcode = "8700" },
                ActiveUser = true,
                FinancialBalance = 1200,
                PaymentDueDate = new DateTime(2021 - 10 - 08),
                UserName = "testusername2",
                Email = "test2@user.dk"
            };

            var machine1 = new Machine()
            {
                Type = "Washer",
                ModelNumber = "123456789dt",
                InstallationDate = new DateTime(2021 - 10 - 08)
            };
            var machine2 = new Machine()
            {
                Type = "Dryer",
                ModelNumber = "123456789ht",
                InstallationDate = new DateTime(2021 - 10 - 08)
            };


            var admin1 = new UserAdmin()
            {
                Name = "Tester1",
                PaymentMethod = "Cash",
                Machines = new List<Machine>() { machine1, machine2 },
                Users = new List<LaundryUser>() { user1, user2 },
                FinancialBalance = 1200,
                PaymentDueDate = new DateTime(2021 - 08 - 08),
                Email = "test@test.dk",
                EmailConfirmed = true
            };

            _context.UserAdmins.Add(admin1);

            _context.SaveChanges();

            var message = new MessageToUserAdmin()
            {
                isRead = false,
                LaundryUser = user1,
                MessageInfo = "Test",
                SendDate = DateTime.Now
            };

            _context.MessageList.Add(message);
            _context.SaveChanges();

        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }


        #endregion
    }
}
