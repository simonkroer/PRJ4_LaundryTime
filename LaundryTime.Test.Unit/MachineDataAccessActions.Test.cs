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
    public class MachineDataAccessActions
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
        public void GetAllMachines_Expected2_True()
        {
            var temp = _uut.Machines.GetAllMachines();
            Assert.That(temp.Count == 2);

            //Check 1st machine
            Assert.That(temp[0].Type == "Washer");
            Assert.That(temp[0].ModelNumber == "10D-505-TR1");
            Assert.That(temp[0].InstallationDate == new DateTime(2020 - 05 - 01));

            // Check 2nd machine
            Assert.That(temp[1].Type == "Dryer");
            Assert.That(temp[1].ModelNumber == "10D-505-TR2");
            Assert.That(temp[1].InstallationDate == new DateTime(2020 - 05 - 01));
            Dispose();
        }

        [Test]
        public void GetSingleMachine_ExpectedTrue_True()
        {
            var temp = _uut.Machines.GetSingleMachine(1);

            Assert.That(temp.Type == "Washer");
            Assert.That(temp.ModelNumber == "10D-505-TR1");
            Assert.That(temp.InstallationDate == new DateTime(2020 - 05 - 01));

            Dispose();
        }

        [Test]
        public void AddMachine_ExpectedMachineAdded_True()
        {
            var temp = _uut.Machines.GetAllMachines();
            Assert.That(temp.Count == 2);

            var machine3 = new Machine()
            {
                Type = "Washer",
                ModelNumber = "10D-505-TR2",
                InstallationDate = new DateTime(2021 - 05 - 15)
            };

            _uut.Machines.AddMachine(machine3);
            _uut.Complete();
            temp = _uut.Machines.GetAllMachines();
            Assert.That(temp.Count == 3);
            Dispose();
        }

        [Test]
        public void DelMachine_ExpectedMachineDeleted_True()
        {
            var machine3 = new Machine()
            {
                Type = "Washer",
                ModelNumber = "10D-505-TR2",
                InstallationDate = new DateTime(2021 - 05 - 15)
            };
            _uut.Machines.AddMachine(machine3);
            _uut.Complete();

            var temp = _uut.Machines.GetAllMachines();
            Assert.That(temp.Count == 3);

            _uut.Machines.DelMachine(3);
            _uut.Complete();
            temp = _uut.Machines.GetAllMachines();
            

            Assert.That(temp.Count == 2);
            Dispose();
        }

        [Test]
        public void GetNumberOfMachines_Expected2_True()
        {
            var temp = _uut.Machines.GetAllMachines();
            Assert.That(temp.Count == 2);
            Dispose();
        }

        [Test]
        public void GetTypeOfMachine_ExpectedWasher_True()
        {
            var temp = _uut.Machines.GetTypeOfMachine(1);

            Assert.That(temp == "Washer");
            Dispose();
        }

        [Test]
        public void MachineExist_ExpectedOneTrueOneFalse()
        {
            var isThere = _uut.Machines.MachineExist("10D-505-TR1");
            Assert.True(isThere);

            var isNotThere = _uut.Machines.MachineExist("testest");
            Assert.False(isNotThere);
        }

        public void Seed()
        {
            _context.Database.EnsureCreated();

            var machine1 = new Machine()
            {
                Type = "Washer",
                ModelNumber = "10D-505-TR1",
                InstallationDate = new DateTime(2020 - 05 - 01)
            };
            _uut.Machines.AddMachine(machine1);
            _uut.Complete();
            var machine2 = new Machine()
            {
                Type = "Dryer",
                ModelNumber = "10D-505-TR2",
                InstallationDate = new DateTime(2020 - 05 - 01)
            };
            _uut.Machines.AddMachine(machine2);
            _uut.Complete();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
