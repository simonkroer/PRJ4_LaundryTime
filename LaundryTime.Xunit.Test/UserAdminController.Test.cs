using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using LaundryTime.Controllers;
using LaundryTime.Data;
using LaundryTime.Data.Models;
using LaundryTime.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace LaundryTime.Xunit.Test
{
    public class UserAdminControllerTest
    {
        protected ApplicationDbContext _context { get; set; }
        protected UserAdminController _uut;

        public UserAdminControllerTest()
        {
            _context = new ApplicationDbContext(
                new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(CreateInMemoryDatabase()).Options);

            Seed();

            _uut = new UserAdminController(_context);
            
            _uut._userAdminViewModel = Substitute.For<UserAdminViewModel>();
            
        }

        #region Index
        [Fact]
        public void Index_AuthorizedUser_ExpectedIActionResult()
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

            var res = _uut.Index();

            Assert.IsType<Task<IActionResult>>(res);
            Dispose();
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

        [Fact]
        public void Index_NotAuthorizedUser_Expected_Unauthorized()
        {
            _uut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim("LaundryUser", "IsLaundryUser")
                    }))
                }
            };

            var res = _uut.Index();

            Assert.IsType<UnauthorizedResult>(res);
            Dispose();
        }


        #endregion

        #region MyUsers
        [Fact]
        public void MyUsers_AuthorizedUser_ExpectedTaskIActionResult()
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

            var res = _uut.MyUsers("", "");

            Assert.IsType<Task<IActionResult>>(res);
            Assert.Equal((int)HttpStatusCode.OK, _uut.ControllerContext.HttpContext.Response.StatusCode);

            Dispose();
        }

        [Fact]
        public async Task MyUsers_AuthorizedUser_ExpectedViewNameCorrect_ModelNotNull()
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

            var res = await _uut.MyUsers("", "") as ViewResult;
            var viewname = res.ViewName;
            var temp = res.Model;

            Assert.True(string.IsNullOrEmpty(viewname) || viewname == "MyUsers");
            Assert.NotNull(temp);

            Dispose();
        }

        [Fact]
        public void MyUsers_NotAuthorizedUser_Expected_Unauthorized()
        {
            _uut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim("LaundryUser", "IsLaundryUser")
                    }))
                }
            };

            var res = _uut.MyUsers("", "");

            Assert.IsType<UnauthorizedResult>(res.Result);
            Dispose();
        }


        #endregion

        #region SortDate
        [Fact]
        public void SortDate_AuthorizedUser_ExpectedRedirectToAction()
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

            var res = _uut.SortDate() as RedirectToActionResult;

            Assert.NotNull(res);
            Assert.Equal("MyUsers", res.ActionName);
            Assert.Equal("sort", res.RouteValues.Values.First());
            Dispose();
        }


        #endregion

        #region SortName
        [Fact]
        public void SortName_AuthorizedUser_ExpectedRedirectToAction()
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

            var res = _uut.SortName() as RedirectToActionResult;

            Assert.NotNull(res);
            Assert.Equal("MyUsers", res.ActionName);
            Assert.Equal("", res.RouteValues.Values.First());
            Dispose();
        }
        #endregion

        #region SearchUser
        [Fact]
        public void SearchUser_AuthorizedUser_ExpectedRedirectToAction()
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

            var res = _uut.SearchUser("user") as RedirectToActionResult;

            Assert.NotNull(res);
            Assert.Equal("MyUsers", res.ActionName);
            Assert.Equal("user", res.RouteValues.Values.First());
            Assert.Equal("", res.RouteValues.Values.ElementAt(1));
            Dispose();
        }


        #endregion

        #region GenerateMyUsersReport
        [Fact]
        public async Task GenerateMyUsersReport_AuthorizedUser_ExpectedFilestreamResult()
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

            var res = await _uut.GenerateMyUsersReport();

            Assert.IsType<FileStreamResult>(res);
            Assert.Equal((int) HttpStatusCode.OK, _uut.ControllerContext.HttpContext.Response.StatusCode);

            Dispose();
        }

        [Fact]
        public async Task GenerateMyUsersReport_AuthorizedUser_ExpectedFileContentNotNull()
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

            var res = await _uut.GenerateMyUsersReport();

            Assert.IsType<FileStreamResult>(res);

            Dispose();
        }

        [Fact]
        public void GenerateMyUsersReport_NotAuthorizedUser_ExpectedDownloadFile()
        {
            _uut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim("LaundryUser", "IsLaundryUser")
                    }))
                }
            };

            var res = _uut.GenerateMyUsersReport();

            Assert.IsType<UnauthorizedResult>(res.Result);

            Dispose();
        }


        #endregion

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

            _context.UserAdmins.Add(admin1);

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
