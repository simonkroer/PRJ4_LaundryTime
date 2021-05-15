using LaundryTime.Data;
using LaundryTime.Data.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaundryTime.Test.Unit
{
    public class LaundryLogDataAccessActions
    {
        protected ApplicationDbContext _context { get; set; }
        protected IDataAccessAction _uut { get; set; }

        static DbConnection CreateInMemoryDatabase()
        {
            var connection = new SqliteConnection("Filename=:memory:");//Fake db
            connection.Open();
            return connection;
        }

        [SetUp]
        public void Setup()
        {
            _context = new ApplicationDbContext(
                new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(CreateInMemoryDatabase()).Options);

            _uut = new DataAccsessAction(_context);

            Seed();
        }

        [Test]
        public void LaundryLogExists_ExpectedTrue_True()
        {
            var temp = _uut.LaundryLogs.LaundryLogExists("Logtest1");

            Assert.True(temp);

            Dispose();
        }

        [Test] 
        public void AddLaundryLog_AddedCorrectly_True()
        {
            var logToAdd = new LaundryLog()
            {
                LogInfo = "LogTest2",
                LogDate = new DateTime(2021 - 10 - 10)
            };
            _uut.LaundryLogs.AddLaundryLog(logToAdd);
            _uut.Complete();

            var temp = _uut.LaundryLogs.LaundryLogExists("LogTest2");
            Assert.True(temp);

            Dispose();
        }

        public void Seed()
        {
            _context.Database.EnsureCreated();
            //var admin1 = new UserAdmin()
            //{
            //    Name = "Tester1",
            //    PaymentMethod = "Cash",
            //    FinancialBalance = 1200,
            //    PaymentDueDate = new DateTime(2021 - 08 - 08),
            //    Email = "test3@test.dk",
            //    UserName = "test3@test.dk",
            //    EmailConfirmed = true
            //};
            //_uut.UserAdmins.AddUserAdmin(admin1);
            //_uut.Complete();

            //var user1 = new LaundryUser()
            //{
            //    Name = "Peter Pedal",
            //    PaymentMethod = "cash",
            //    Address = new Address() { Country = "Denmark", StreetAddress = "Testvej 1", Zipcode = "8700" },
            //    ActiveUser = true,
            //    FinancialBalance = 1200,
            //    PaymentDueDate = new DateTime(2021 - 10 - 08)
            //};
            //_uut.LaundryUsers.AddLaundryUser(user1);
            //_uut.Complete();

            var log1 = new LaundryLog()
            {
                LogInfo = "Logtest1",
                LogDate = new DateTime(2021 - 05 - 15)
            };
            _uut.LaundryLogs.AddLaundryLog(log1);
            _uut.Complete();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
