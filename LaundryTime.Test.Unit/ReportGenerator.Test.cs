using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaundryTime.Data;
using LaundryTime.Data.Models;
using LaundryTime.Utilities;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NUnit.Framework;

namespace LaundryTime.Test.Unit
{
    public class ReportGenerator
    {
        public List<LaundryUser> laundryusers { get; set; }
        public List<Machine> machines { get; set; }
        public List<int> randomlist { get; set; }
        public IReportGenerator _uut { get; set; }

        [SetUp]
        public void Setup()
        {
            Seed();

            _uut = new Utilities.ReportGenerator();
        }

        [Test]
        public void GenerateReport_With_LuandryUser_Content()
        {
            var temp = _uut.GenerateReport(laundryusers);

            Assert.That(temp.FileName.Equals("Report.json"));
            Assert.That(temp.Content, Is.Not.Null);
            Assert.That(temp.Format.Equals("text/json"));
        }
        [Test]
        public void GenerateUsersReport_Empty_Laundryuser_content_EmptyReportExpected()
        {
            laundryusers = new List<LaundryUser>();

            var temp = _uut.GenerateReport(laundryusers);

            Assert.That(temp, Is.Not.Null);
        }

        [Test]
        public void GenerateReport_With_Machine_Content()
        {
            var temp = _uut.GenerateReport(machines);

            Assert.That(temp.FileName.Equals("Report.json"));
            Assert.That(temp.Content, Is.Not.Null);
            Assert.That(temp.Format.Equals("text/json"));
        }
        [Test]
        public void GenerateReport_Empty_Machine_Content_ExpectedEmptyReportObject()
        {
            machines = new List<Machine>();

            var temp = _uut.GenerateReport(machines);

            Assert.That(temp, Is.Not.Null);
        }

        [Test]
        public void GenerateReport_Random_Content()
        {
            var temp = _uut.GenerateReport(randomlist);

            Assert.That(temp.Content, Is.Null);
            Assert.That(temp.FileName, Is.Null);
            Assert.That(temp.Format, Is.Null);
        }

        void Seed()
        {
            laundryusers = new List<LaundryUser>()
            {
                new LaundryUser()
                {
                    Name = "TestUser",
                    PaymentMethod = "Mobile",
                    LaundryHistory = Substitute.For<List<LaundryLog>>(),
                    Address = Substitute.For<Address>(),
                },
                new LaundryUser()
                {
                    Name = "TestUser2",
                    PaymentMethod = "Mobile2",
                    LaundryHistory = Substitute.For<List<LaundryLog>>(),
                    Address = Substitute.For<Address>(),
                }
            };

            machines = new List<Machine>()
            {
                new Machine()
                {
                    InstallationDate = new DateTime(2021-10-08),
                    ModelNumber = "123456asdas",
                    Type = "Washer",
                    UserAdmin = Substitute.For<UserAdmin>()
                },
                new Machine()
                {
                    InstallationDate = new DateTime(2021-10-05),
                    ModelNumber = "123333456asdas",
                    Type = "Dyer",
                    UserAdmin = Substitute.For<UserAdmin>()
                }
            };

            randomlist = new List<int>() { 1, 2, 3 };
        }
    }
}
