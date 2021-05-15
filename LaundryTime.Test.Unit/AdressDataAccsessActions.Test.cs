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
    public class AdressDataAccsessActions
    {
        protected ApplicationDbContext _context { get; set; }
        protected IDataAccessAction _uut { get; set; }

        [SetUp]
        public void Setup()
        {
            _context = new ApplicationDbContext(
                new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(CreateInMemoryDatabase()).Options);

            _uut = new DataAccsessAction(_context);

            Seed();
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

            var adr1 = new Address()
            {
                StreetAddress = "Address1",
                Zipcode = "1000",
                Country = "Denmark"
            };

            var adr2 = new Address()
            {
                StreetAddress = "Address2",
                Zipcode = "2000",
                Country = "Spain"
            };

            _context.Addresses.AddRange(adr1, adr2);
            _context.SaveChanges();
        }

        [Test]
        public void GetAllAddresses_Expected2_True()
        {
            var temp = _uut.Addresses.GetAllAdresses();

            Assert.That(temp.Count == 2);
            Assert.That(temp, Is.TypeOf<List<Address>>());

            Dispose();
        }

        [Test]
        public void GetSingleAdress_ExpectedTrue_True()
        {
            Address temp = _uut.Addresses.GetSingleAddress(1);

            Assert.That(temp.StreetAddress, Is.EqualTo("Address1"));
            Assert.That(temp.Zipcode, Is.EqualTo("1000"));
            Assert.That(temp.Country, Is.EqualTo("Denmark"));
        }

        [Test]
        public void AddAddress_ExpectedAddressAdded_True()
        {
            var addressToAdd = new Address()
            {
                StreetAddress = "Address3",
                Zipcode = "3000",
                Country = "Sweden"
            };
            _uut.Addresses.AddAddress(addressToAdd);
            _uut.Complete();

            var temp = _uut.Addresses.GetAllAdresses();
            Assert.That(temp.Count == 3);

            var newAdressToCheck = _uut.Addresses.GetSingleAddress(3);
            Assert.That(newAdressToCheck.StreetAddress, Is.EqualTo("Address3"));
            Assert.That(newAdressToCheck.Zipcode, Is.EqualTo("3000"));
            Assert.That(newAdressToCheck.Country, Is.EqualTo("Sweden"));

            Dispose();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
