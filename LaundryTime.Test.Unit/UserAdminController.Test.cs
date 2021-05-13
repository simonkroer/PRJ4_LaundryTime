using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaundryTime.Data;
using LaundryTime.ViewModels;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NUnit.Framework;

namespace LaundryTime.Test.Unit
{
    public class UserAdminController
    {
        protected ApplicationDbContext _context { get; set; }
        protected UserAdminController _userAdminController;
        private IDataAccessAction _dataAccess;
        public UserAdminViewModel _userAdminViewModel;

        [SetUp]
        public void Setup()
        {
            _context = new ApplicationDbContext(
                new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(CreateInMemoryDatabase()).Options);

            Seed();

            _dataAccess = Substitute.For<IDataAccessAction>();
            _userAdminViewModel = Substitute.For<UserAdminViewModel>();

            _userAdminController = new UserAdminController();
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
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

            //Seeding goes here

            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
