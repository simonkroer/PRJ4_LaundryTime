using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using LaundryTime.Data;
using LaundryTime.Data.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace LaundryTime.Test.Unit
{
    public class Tests
    {
        protected ApplicationDbContext _context { get; set; }
        protected IDataAccessAction _dataAccess { get; set; }

        [SetUp]
        public void Setup()
        {
            _context = new ApplicationDbContext(
                new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(CreateInMemoryDatabase()).Options);

            _dataAccess = new DataAccsessAction(_context);

            Seed();
        }

        [Test]
        public void UserExists_Expected_true()
        {

            var temp = _dataAccess.UserAdmins.UserExists("test@test.dk");

            Assert.True(temp);

            Dispose();
        }

        [Test]
        public void GetAllUserAdmins_Expected_2()
        {
            var temp = _dataAccess.UserAdmins.GetAllUserAdmins();


            Assert.That(temp.Count==2);
            Assert.That(temp, Is.TypeOf<List<UserAdmin>>());
            Assert.That(temp[0].Machines, Is.Not.Null);
            Assert.That(temp[0].Users, Is.Not.Null);

            Dispose();
        }

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
                Machines = new List<Machine>() { machine1, machine2 },
                Users = new List<LaundryUser>() { user1, user2 },
                FinancialBalance = 1200,
                PaymentDueDate = new DateTime(2021 - 08 - 08),
                Email = "test@test.dk",
                EmailConfirmed = true
            };

            var admin2 = new UserAdmin()
            {
                Name = "Tester2",
                PaymentMethod = "Mobilepay",
                Machines = new List<Machine>() { machine1, machine2 },
                Users = new List<LaundryUser>() { user1, user2 },
                FinancialBalance = 1400,
                PaymentDueDate = new DateTime(2021 - 10 - 08),
                Email = "test2@test.dk",
                EmailConfirmed = true
            };

            _context.UserAdmins.AddRange(admin1,admin2);
            _context.SaveChanges();

        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}