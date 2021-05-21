using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using LaundryTime.Controllers;
using LaundryTime.Data;
using LaundryTime.Data.Models;
using LaundryTime.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NUnit.Framework;
using Xunit;

namespace LaundryTime.Test.Unit
{
    class XunitTest.Test
    {
        protected ApplicationDbContext _context { get; set; }
        protected UserAdminController _uut;

        public XunitTestTest()
        {
            _context = new ApplicationDbContext(
                new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(CreateInMemoryDatabase()).Options);

            Seed();

            _uut = new UserAdminController(_context);

            _uut._userAdminViewModel = Substitute.For<UserAdminViewModel>();
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
                        new Claim("UserAdmin", "IsUserAdmin")
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
