using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using LaundryTime.Areas.Identity.Pages.Account;
using LaundryTime.Controllers;
using LaundryTime.Data;
using LaundryTime.Data.Models;
using LaundryTime.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework.Internal;
using Twilio.TwiML.Voice;
using Xunit;
using Task = System.Threading.Tasks.Task;

/*
namespace LaundryTime.Xunit.Test
{
    public class SystemAdminControllerTest
    {
        protected ApplicationDbContext _context { get; set; }
        protected SystemAdminController _uut;

        public SystemAdminControllerTest()
        {
            _context = new ApplicationDbContext(
                new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(CreateInMemoryDatabase()).Options);

            Seed();

            _uut._systemAdminViewModel = Substitute.For<SystemAdminViewModel>();
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
                        new Claim("SystemAdmin", "IsSystemAdmin")
                    }))
                }
            };

            var res = _uut.Index();

            Assert.IsType<ViewResult>(res);
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
                        new Claim("SystemAdmin", "IsSystemAdmin")
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
                        new Claim("SystemAdmin", "IsSystemAdmin")
                    }))
                }
            };

            //var res = _uut.MyUsers("", "");

            //Assert.IsType<Task<IActionResult>>(res);
            Assert.Equal((int)HttpStatusCode.OK, _uut.ControllerContext.HttpContext.Response.StatusCode);

            Dispose();
        }

        [Fact]
        public async Task MyAdminUsers_AuthorizedUser_ExpectedViewNameCorrect_ModelNotNull()
        {
            _uut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim("SystemAdmin", "IsSystemAdmin")
                    }))
                }
            };

			var res = await _uut.MyAdminUsers("", "") as ViewResult;
			var viewname = res.ViewName;
			var temp = res.Model;

			Assert.True(string.IsNullOrEmpty(viewname) || viewname == "MyUsers");
			Assert.NotNull(temp);

			Dispose();
        }

        [Fact]
        public void MySystemAdminUsers_NotAuthorizedUser_Expected_Unauthorized()
        {
            _uut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim("SystemAdmin", "IsSystemAdmin")
                    }))
                }
            };

            //var res = _uut.MyUsers("", "");

            //Assert.IsType<UnauthorizedResult>(res.Result);
            Dispose();
        }


		#endregion

		#region SortDate (Bliver ikke brugt af SystemAdmin)
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

	#region SortName (Bliver ikke brugt af SystemAdmin)
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
        public void GenerateMyUsersReport_NotAuthorizedUser_ExpectedNotAuthorized()
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

        #region GenerateMyMachinesReport
        [Fact]
        public async Task GenerateMyMachinesReport_AuthorizedUser_ExpectedFilestreamResult()
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

            var res = await _uut.GenerateMyMachinesReport();

            Assert.IsType<FileStreamResult>(res);
            Assert.Equal((int)HttpStatusCode.OK, _uut.ControllerContext.HttpContext.Response.StatusCode);

            Dispose();
        }

        [Fact]
        public void GenerateMyMachinesReport_NotAuthorizedUser_ExpectedUnAuthorized()
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

            var res = _uut.GenerateMyMachinesReport();

            Assert.IsType<UnauthorizedResult>(res.Result);

            Dispose();
        }


        #endregion

        #region DeleteUserAdmin
        [Fact]
        public void DeleteUser_AuthorizedUser_ExpectedRedirectToAction()
        {
            _uut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim("SystemAdmin", "ISystemAdmin")
                    }))
                }
            };

            var res = _uut.DeleteUser("testusername1") as RedirectToActionResult;

            Assert.NotNull(res);
            Assert.Equal("MyUsers", res.ActionName);
            Assert.Equal((int)HttpStatusCode.OK, _uut.ControllerContext.HttpContext.Response.StatusCode);

            Dispose();
        }

        [Fact]
        public void DeleteUser_NotAuthorizedUser_ExpectedUnAuthorized()
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

            var res = _uut.DeleteUser("testusername2");

            Assert.IsType<UnauthorizedResult>(res);

            Dispose();
        }


        #endregion

        #region EditUser
        [Fact]
        public void EditUser_AuthorizedUser_ExpectedResponse_OK()
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

            var res = _uut.EditUser("test1@user.dk");

            Assert.NotNull(res);
            Assert.Equal((int)HttpStatusCode.OK, _uut.ControllerContext.HttpContext.Response.StatusCode);

            Dispose();
        }

        [Fact]
        public void EditUser_AuthorizedUser_Expected_ViewNameCorrect_ModelNotNull()
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

            var res = _uut.EditUser("testusername1") as ViewResult;
            var viewname = res.ViewName;
            var tempmodel = res.Model;

            Assert.True(string.IsNullOrEmpty(viewname) || viewname == "EditUser");
            Assert.NotNull(tempmodel);

            Dispose();
        }

        [Fact]
        public void EditUser_NotAuthorizedUser_Expected_Unauthorized()
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

            var res = _uut.EditUser("testusername1");

            Assert.IsType<UnauthorizedResult>(res);
            Dispose();
        }

        [Fact]
        public void EditUser_AuthorizedUser_Expected_Notfound()
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

            var res = _uut.EditUser("testusername300");

            Assert.IsAssignableFrom<NotFoundResult>(res);
            Dispose();
        }
        #endregion

        #region UpdateUser
        [Fact]
        public void UpdateUsers_AuthorizedUser_ExpectedRedirectToActionResult()
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
            _uut._userAdminViewModel.CurrentLaundryUser = _context.LaundryUsers.SingleOrDefault(d=>d.UserName == "testusername1");

            var res = _uut.UpdateUser(_uut._userAdminViewModel);
            
            Assert.NotNull(res);
            Assert.Equal((int)HttpStatusCode.OK, _uut.ControllerContext.HttpContext.Response.StatusCode);
            Assert.IsType<RedirectToActionResult>(res);

            Dispose();
        }

        [Fact]
        public void UpdateUsers_AuthorizedUser_ExpectedViewNameCorrect()
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
            _uut._userAdminViewModel.CurrentLaundryUser = _context.LaundryUsers.SingleOrDefault(d => d.UserName == "testusername1");

            var res = _uut.UpdateUser(_uut._userAdminViewModel) as RedirectToActionResult;
            var viewname = res.ActionName;

            Assert.True(string.IsNullOrEmpty(viewname) || viewname == "MyUsers");

            Dispose();
        }
        [Fact]
        public void UpdateUsers_NotAuthorizedUser_Expected_Unauthorized()
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
            _uut._userAdminViewModel.CurrentLaundryUser = _context.LaundryUsers.SingleOrDefault(d => d.UserName == "testusername1");

            var res = _uut.UpdateUser(_uut._userAdminViewModel);

            Assert.IsType<UnauthorizedResult>(res);
            Dispose();
        }


        #endregion

        #region ToggleBlockUser
        [Fact]
        public void ToggleBlockUser_AuthorizedUser_ExpectedRedirectToActionResult()
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
            _uut._userAdminViewModel.CurrentLaundryUser = _context.LaundryUsers.SingleOrDefault(d => d.UserName == "testusername1");
            _uut.TempData = new TempDataDictionary(new DefaultHttpContext(), Substitute.For<ITempDataProvider>());

            var res = _uut.ToggleBlockUser(_uut._userAdminViewModel) as RedirectToActionResult;

            Assert.NotNull(res);
            Assert.Equal((int)HttpStatusCode.OK, _uut.ControllerContext.HttpContext.Response.StatusCode);
            Assert.Equal("EditUser", res.ActionName);

            Dispose();
        }

        [Fact]
        public void ToggleBlockUser_AuthorizedUser_ExpectedViewNameCorrect()
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
            _uut._userAdminViewModel.CurrentLaundryUser = _context.LaundryUsers.SingleOrDefault(d => d.UserName == "testusername1");
            _uut.TempData = new TempDataDictionary(new DefaultHttpContext(), Substitute.For<ITempDataProvider>());

            var res = _uut.ToggleBlockUser(_uut._userAdminViewModel) as RedirectToActionResult;
            var viewname = res.ActionName;

            Assert.True(string.IsNullOrEmpty(viewname) || viewname == "EditUser");

            Dispose();
        }
        [Fact]
        public void ToggleBlockUser_NotAuthorizedUser_Expected_Unauthorized()
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
            _uut._userAdminViewModel.CurrentLaundryUser = _context.LaundryUsers.SingleOrDefault(d => d.UserName == "testusername1");
            _uut.TempData = new TempDataDictionary(new DefaultHttpContext(), Substitute.For<ITempDataProvider>());

            var res = _uut.ToggleBlockUser(_uut._userAdminViewModel);

            Assert.IsType<UnauthorizedResult>(res);
            Dispose();
        }
        #endregion

        #region IndexMachines
        //[Fact]
        //public void IndexMachines_AuthorizedUser_ExpectedIActionResult()
        //{
        //    _uut.ControllerContext = new ControllerContext
        //    {
        //        HttpContext = new DefaultHttpContext
        //        {
        //            User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        //            {
        //                new Claim("UserAdmin", "IsUserAdmin")
        //            }))
        //        }
        //    };

        //    var res = _uut.IndexMachines();

        //    Assert.IsType<ViewResult>(res);
        //    Dispose();
        //}

        [Fact]
        public void IndexMachines_AuthorizedUser_Expected_ViewNameCorrect_ModelNotNull()
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

            var res = _uut.IndexMachines().Result as ViewResult;
            var viewname = res.ViewName;
            var temp = res.Model;

            Assert.True(string.IsNullOrEmpty(viewname) || viewname == "IndexMachines");
            Assert.NotNull(temp);

            Dispose();
        }

        [Fact]
        public void IndexMachines_NotAuthorizedUser_Expected_Unauthorized()
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

            var res = _uut.IndexMachines();

            Assert.IsType<UnauthorizedResult>(res);
            Dispose();
        }


        #endregion

        #region AddMachines
        [Fact]
        public void AddMachines_NoParameter_AuthorizedUser_ExpectedViewResult_NewMachine()
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
            _uut._userAdminViewModel.CurrentLaundryUser = _context.LaundryUsers.SingleOrDefault(d => d.UserName == "testusername1");

            var res = _uut.AddMachines();

            Assert.NotNull(res);
            Assert.Equal((int)HttpStatusCode.OK, _uut.ControllerContext.HttpContext.Response.StatusCode);
            Assert.IsType<ViewResult>(res);

            Dispose();
        }
        [Fact]
        public void AddMachines_NoParameter_AuthorizedUser_Expected_ViewNameCorrect_ModelNotNull()
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

            var res = _uut.AddMachines() as ViewResult;
            var viewname = res.ViewName;
            var temp = res.Model;

            Assert.True(string.IsNullOrEmpty(viewname) || viewname == "AddMachines");
            Assert.NotNull(temp);

            Dispose();
        }

        [Fact]
        public void AddMachines_NoParameter_NotAuthorizedUser_Expected_Unauthorized()
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

            var res = _uut.AddMachines();

            Assert.IsType<UnauthorizedResult>(res);
            Dispose();
        }
        [Fact]
        public void AddMachines_withParameter_NotAuthorizedUser_Expected_Unauthorized()
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
            _uut._userAdminViewModel.CurrentLaundryUser = _context.LaundryUsers.SingleOrDefault(d => d.UserName == "testusername1");

            var res = _uut.AddMachines(_uut._userAdminViewModel);

            Assert.IsType<UnauthorizedResult>(res);
            Dispose();
        }

        //[Fact]
        //public void AddMachines_WithParameter_AuthorizedUser_ExpectedRedirect_NewMachine()
        //{
        //_uut.ControllerContext = new ControllerContext
        //{
        //    HttpContext = new DefaultHttpContext
        //    {
        //        User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        //        {
        //            new Claim("UserAdmin", "IsUserAdmin"),
        //            new Claim(ClaimTypes.Name, "Tester1")
        //        }))
        //    }
        //};

        //    _uut.ControllerContext.HttpContext.User.AddIdentity(new ClaimsIdentity(new Claim[]{new Claim(ClaimTypes.Name,"Tester1"), new Claim("UserAdmin", "IsUserAdmin") }));

        //    _uut._userAdminViewModel.CurrentLaundryUser = _context.LaundryUsers.SingleOrDefault(d => d.UserName == "testusername1");
        //    _uut._userAdminViewModel.CurrentMachine = _context.Machines.SingleOrDefault(d=>d.ModelNumber== "123456789dt");
        //    _uut.TempData = new TempDataDictionary(new DefaultHttpContext(), Substitute.For<ITempDataProvider>());

        //    var res = _uut.AddMachines(_uut._userAdminViewModel);

        //    Assert.NotNull(res);
        //    //Assert.Equal((int)HttpStatusCode.OK, _uut.ControllerContext.HttpContext.Response.StatusCode);
        //    //Assert.IsType<RedirectToActionResult>(res);

        //    Dispose();
        //}
        #endregion

        #region DeleteMachines
        [Fact]
        public void DeleteMachines_AuthorizedUser_ExpectedRedirect_NotNull()
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
            _uut._userAdminViewModel.CurrentLaundryUser = _context.LaundryUsers.SingleOrDefault(d => d.UserName == "testusername1");

            var res = _uut.DeleteMachines("1") as RedirectToActionResult;

            Assert.NotNull(res);
            Assert.Equal((int)HttpStatusCode.OK, _uut.ControllerContext.HttpContext.Response.StatusCode);
            Assert.Equal("IndexMachines", res.ActionName);

            Dispose();
        }

        [Fact]
        public void DeleteMachines_NotAuthorizedUser_Expected_UnAuthorized()
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

            var res = _uut.DeleteMachines("1");

            Assert.NotNull(res);
            Assert.IsType<UnauthorizedResult>(res);

            Dispose();
        }


        #endregion

        #region GetMessages
        [Fact]
        public void GetMessages_AuthorizedUser_ExpectedViewResult()
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
            _uut._userAdminViewModel.CurrentLaundryUser = _context.LaundryUsers.SingleOrDefault(d => d.UserName == "testusername1");

            var res = _uut.GetMessages();

            Assert.NotNull(res);
            Assert.Equal((int)HttpStatusCode.OK, _uut.ControllerContext.HttpContext.Response.StatusCode);
            Assert.IsType<ViewResult>(res);

            Dispose();
        }

        [Fact]
        public void GetMessages_AuthorizedUser_Expected_ModelNotNUll()
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
            _uut._userAdminViewModel.CurrentLaundryUser = _context.LaundryUsers.SingleOrDefault(d => d.UserName == "testusername1");

            var res = _uut.GetMessages() as ViewResult;

            Assert.NotNull(res.Model);

            Dispose();
        }

        [Fact]
        public void GetMessages_NotAuthorizedUser_Expected_Unauthorized()
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

            var res = _uut.GetMessages();

            Assert.NotNull(res);
            Assert.IsType<UnauthorizedResult>(res);

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
                PaymentDueDate = new DateTime(2021 - 10 - 08),
                UserName = "testusername1",
                Email = "test1@user.dk"
            };
            var user2 = new LaundryUser()
            {
                Name = "testerlaundryuser2",
                PaymentMethod = "mobilepay",
                Address = new Address() { Country = "Denmark", StreetAddress = "Testvej 1", Zipcode = "8700" },
                ActiveUser = true,
                FinancialBalance = 1200,
                PaymentDueDate = new DateTime(2021 - 10 - 08),
                UserName = "testusername2",
                Email = "test2@user.dk"
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

            var message = new MessageToUserAdmin()
            {
                isRead = false,
                LaundryUser = user1,
                MessageInfo = "Test",
                SendDate = DateTime.Now
            };

            _context.MessageList.Add(message);
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
/*