using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Security.Claims;
using System.Threading.Tasks;
using LaundryTime.Controllers;
using LaundryTime.Data;
using LaundryTime.Data.Models;
using LaundryTime.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Xunit;

namespace LaundryTime.Xunit.Test
{
    public class UserAdminControllerTest
    {
        protected ApplicationDbContext _context { get; set; }
        protected UserAdminController _userAdminController;
        private IDataAccessAction _dataAccessfake;
        public UserAdminViewModel _userAdminViewModelfake;
        public static UserManager<ApplicationUser> _userManager { get; set; }

        public UserAdminControllerTest()
        {

        }


        //[Fact]
        //public async Task MyUsers_Expected()
        //{
        //    //Arrange:
        //    _context = new ApplicationDbContext(
        //        new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(CreateInMemoryDatabase()).Options);
        //    Seed();
        //    _dataAccessfake = Substitute.For<DataAccsessAction>(_context);
        //    _userAdminViewModelfake = Substitute.For<UserAdminViewModel>();
        //    _userAdminController = new UserAdminController(_context);
        //    var _usermanagerfake = Substitute.For<UserManager<ApplicationUser>>();
        //    UserAdmin user = await _context.UserAdmins.SingleOrDefaultAsync(); //Get user

        //    //Login

        //    //Act:
        //    var res = await _userAdminController.MyUsers();

        //    //Assert:
        //    //var viewres = Assert.IsType<ViewResult>(res);

        //    Dispose();
        //}

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
                PaymentDueDate = new DateTime(2021 - 10 - 08)
            };
            var user2 = new LaundryUser()
            {
                Name = "testerlaundryuser2",
                PaymentMethod = "mobilepay",
                Address = new Address() { Country = "Denmark", StreetAddress = "Testvej 1", Zipcode = "8700" },
                ActiveUser = true,
                FinancialBalance = 1200,
                PaymentDueDate = new DateTime(2021 - 10 - 08)
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
                Machines = new List<Machine>(){machine1,machine2},
                Users = new List<LaundryUser>(){ user1, user2 },
                FinancialBalance = 1200,
                PaymentDueDate = new DateTime(2021 - 08 - 08),
                Email = "test@test.dk",
                EmailConfirmed = true
            };

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
