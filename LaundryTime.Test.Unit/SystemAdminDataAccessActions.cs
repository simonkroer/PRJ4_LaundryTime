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
    public class SystemAdminDataAccessActions
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
        public void GetAllSystemAdmins_Expected1_True()
        {
            var temp = _uut.SystemAdmins.GetAllSystemAdmins();
            Assert.That(temp.Count == 1);

            Dispose();
        }

        [Test]
        public void GetSingleSystemAdmin_ExpectedTrue_True()
        {
            var temp = _uut.SystemAdmins.GetSingleSystemAdmin("Jefe");

            Assert.That(temp.Name == "Jefe");
            Assert.That(temp.LaundryUsers, Is.Not.Null);
            Assert.That(temp.UserAdmins, Is.Not.Null);
            Assert.That(temp.PhoneNumber == "12345678");
            Assert.That(temp.Email == "test@test.dk");
            Assert.That(temp.EmailConfirmed == true);

            Dispose();
        }

        [Test]
        public void AddSystemAdmin_ExpectedOneAdded_True()
        {
            var temp = _uut.SystemAdmins.GetAllSystemAdmins();
            Assert.That(temp.Count == 1);

            var systemAdmin2 = new SystemAdmin()
            {
                Name = "Nougat",
                UserAdmins = new List<UserAdmin> { new UserAdmin { Name = "admin3"} },
                LaundryUsers = new List<LaundryUser> { new LaundryUser { Name = "testUser1"}, new LaundryUser { Name = "testUser2" } },
                Email = "test2@test2.dk",
                EmailConfirmed = true,
                PhoneNumber = "87654321",
                UserName = "Nougat"
            };
            _uut.SystemAdmins.AddSystemAdmin(systemAdmin2);
            _uut.Complete();

            temp = _uut.SystemAdmins.GetSingleSystemAdmin(systemAdmin2.Name);
            Assert.That(temp == systemAdmin2);

            Dispose();
        }

        [Test]
        public void UserExists_ExpectedTrue_True()
        {
            var temp = _uut.SystemAdmins.UserExists("test@test.dk");
            Assert.True(temp);

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

            _uut.LaundryUsers.AddLaundryUser(user1);
            _uut.LaundryUsers.AddLaundryUser(user2);
            _uut.Complete();

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

            _uut.Machines.AddMachine(machine1);
            _uut.Machines.AddMachine(machine2);
            _uut.Complete();

            var admin1 = new UserAdmin()
            {
                Name = "Tester1",
                PaymentMethod = "Cash",
                Machines = new List<Machine>() { machine1, machine2 },
                Users = new List<LaundryUser>() { user1, user2 },
                FinancialBalance = 1200,
                PaymentDueDate = new DateTime(2021 - 08 - 08),
                Email = "test@test.dk",
                UserName = "test@test.dk",
                EmailConfirmed = true
            };

            _uut.UserAdmins.AddUserAdmin(admin1);
            _uut.Complete();


            var systemAdmin = new SystemAdmin()
            {
                Name = "Jefe",
                UserAdmins = new List<UserAdmin> { admin1 },
                LaundryUsers = new List<LaundryUser> { user1, user2 },
                Email = "test@test.dk",
                EmailConfirmed = true,
                PhoneNumber = "12345678",
                UserName = "Jefe"
            };

            _uut.SystemAdmins.AddSystemAdmin(systemAdmin);
            _uut.Complete();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
