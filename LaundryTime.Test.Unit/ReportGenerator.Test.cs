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
        public List<int> randomlist { get; set; }
        public IReportGenerator _uut { get; set; }

        [SetUp]
        public void Setup()
        {
            Seed();

            _uut = new Utilities.ReportGenerator();
        }

        [Test]
        public void GenerateMyUsersReport_With_Content()
        {
            var temp = _uut.GenerateMyUsersReport(laundryusers);

            Assert.That(temp.FileName.Equals("MyUsersReport.json"));
            Assert.That(temp.Content, Is.Not.Null);
            Assert.That(temp.Format.Equals("text/json"));
        }
        [Test]
        public void GenerateMyMachinesReport_With_Content()
        {
        }
        [Test]
        public void GenerateMyMachinesReport_No_Content()
        {
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
            randomlist = new List<int>() { 1, 2, 3 };
        }
    }
}
