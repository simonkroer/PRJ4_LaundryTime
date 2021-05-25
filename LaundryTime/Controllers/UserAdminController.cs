using LaundryTime.Data;
using LaundryTime.Data.Models;
using LaundryTime.Utilities;
using LaundryTime.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;


namespace LaundryTime.Controllers
{
    public class UserAdminController : Controller 
    {
        private readonly ApplicationDbContext _context;
        private IDataAccessAction _dataAccess;
        public UserAdminViewModel _userAdminViewModel;
        protected IReportGenerator _reportGenerator;

        public UserAdminController(ApplicationDbContext context)
        {
            _context = context;
            _dataAccess = new DataAccsessAction(context);
            _userAdminViewModel = new UserAdminViewModel();
            _reportGenerator = new ReportGenerator();
        }

        public IActionResult Index()
        {
            if (User.HasClaim("UserAdmin", "IsUserAdmin"))
            {
                if (User.Identity != null)
                    _userAdminViewModel.CurrentUserAdmin = _dataAccess.UserAdmins.GetSingleUserAdmin(User.Identity.Name);

                return View(_userAdminViewModel);
            }
            
            return Unauthorized();
        }

        [HttpGet]
        public async Task<IActionResult> MyUsers(string sortDate, string nameinput)
        {
            if (User.Identity != null && (User.HasClaim("UserAdmin", "IsUserAdmin") || User.HasClaim("SystemAdmin", "IsSystemAdmin")))
            {
                if(User.HasClaim("SystemAdmin","IsSystemAdmin"))
                {
                    //var currentuser = await _dataAccess.UserAdmins.GetSingleUserAdminAsync(username);

                    //_userAdminViewModel.MyUsers = currentuser.Users;
                }
                else
                {
                    var currentuser = await _dataAccess.UserAdmins.GetSingleUserAdminAsync(User.Identity.Name);

                    _userAdminViewModel.MyUsers = currentuser.Users;
                }


                if (!string.IsNullOrEmpty(nameinput))
                {
                    _userAdminViewModel.MyUsers = _userAdminViewModel.MyUsers.Where(s => s.Name.Contains(nameinput)).ToList();
                }
                if(sortDate == "sort")
                {
                    _userAdminViewModel.MyUsers.Sort((res1, res2) => res1.PaymentDueDate.CompareTo(res2.PaymentDueDate));
                }
                else
                {
                    _userAdminViewModel.MyUsers.Sort((res1, res2) => String.Compare(res1.Name, res2.Name, StringComparison.Ordinal));
                }

                return View(_userAdminViewModel);
            }

            return Unauthorized();
            
        }

        public IActionResult SortDate()
        {
            return RedirectToAction(nameof(MyUsers), new { sortDate = "sort" });
        }

        public IActionResult SortName()
        {
            return RedirectToAction(nameof(MyUsers), new { sortDate = "" });
        }

        public IActionResult SearchUser(string nameinput)
        {
            return RedirectToAction(nameof(MyUsers), new{nameinput = nameinput, sortDate=""});
        }

        [HttpGet("MyUsersReport")]
        public  async Task<IActionResult> GenerateMyUsersReport()
        {
            if (User.Identity != null && User.HasClaim("UserAdmin", "IsUserAdmin"))
            {
                var currentuser = await _dataAccess.UserAdmins.GetSingleUserAdminAsync(User.Identity.Name);

                var report = _reportGenerator.GenerateReport(currentuser.Users);

                return File(report.Content, report.Format, report.FileName);
                
            }
            return Unauthorized();
        }

        [HttpGet("MyMachinesReport")]
        public async Task<IActionResult> GenerateMyMachinesReport()
        {
            if (User.Identity != null && User.HasClaim("UserAdmin", "IsUserAdmin"))
            {
                var currentuser = await _dataAccess.UserAdmins.GetSingleUserAdminAsync(User.Identity.Name);

                var report = _reportGenerator.GenerateReport(currentuser.Machines);

                return File(report.Content, report.Format, report.FileName);
            }
            return Unauthorized();
        }

        public IActionResult DeleteUser(string username)
        {
            if (User.HasClaim("UserAdmin", "IsUserAdmin") || User.HasClaim("SystemAdmin", "IsSystemAdmin"))
            {
                if (username == null)
                {
                    return NotFound();
                }

                if (_userAdminViewModel.CurrentLaundryUser != null)
                {
                    if (_userAdminViewModel.CurrentLaundryUser.UserName == username || _userAdminViewModel.CurrentLaundryUser.Email == username)
                    {
                        _userAdminViewModel.CurrentLaundryUser = null;
                    }
                }

                var userToDelete = _dataAccess.LaundryUsers.GetSingleLaundryUser(username);

                _dataAccess.LaundryUsers.DeleteUser(userToDelete);
                _dataAccess.Complete();

                return RedirectToAction(nameof(MyUsers));
            }

            return Unauthorized();
        }
        
        [HttpGet]
        public IActionResult EditUser(string email)
        {
            if (User.HasClaim("UserAdmin", "IsUserAdmin") || User.HasClaim("SystemAdmin", "IsSystemAdmin"))
            {
                if (email == null)
                {
                    return NotFound();
                }

                _userAdminViewModel.CurrentLaundryUser = _dataAccess.LaundryUsers.GetSingleLaundryUser(email);

                if (_userAdminViewModel.CurrentLaundryUser == null)
                {
                    return NotFound();
                }

                return View(_userAdminViewModel);
            }
            
            return Unauthorized();
            
                
        }

        [HttpPost]
        public IActionResult UpdateUser(UserAdminViewModel viewModel)
        {
            if (User.HasClaim("UserAdmin", "IsUserAdmin") || User.HasClaim("SystemAdmin", "IsSystemAdmin"))
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var user = _dataAccess.LaundryUsers.GetSingleLaundryUser(viewModel.CurrentLaundryUser.UserName);

                        user.Name = viewModel.CurrentLaundryUser.Name;
                        user.PhoneNumber = viewModel.CurrentLaundryUser.PhoneNumber;
                        user.Email = viewModel.CurrentLaundryUser.Email;

                        if (user.Address == null && viewModel.CurrentLaundryUser.Address != null)
                        {
                            user.Address = new Address();
                            user.Address.StreetAddress = viewModel.CurrentLaundryUser.Address.StreetAddress;
                            user.Address.Zipcode = viewModel.CurrentLaundryUser.Address.Zipcode;
                        }

                        user.PaymentMethod = viewModel.CurrentLaundryUser.PaymentMethod;
                        user.PaymentDueDate = viewModel.CurrentLaundryUser.PaymentDueDate;
                        user.UserName = viewModel.CurrentLaundryUser.UserName;

                        _dataAccess.LaundryUsers.Update(user);
                        _dataAccess.Complete();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!_dataAccess.LaundryUsers.LaundryUserExists(viewModel.CurrentLaundryUser.Email))
                        {
                            return NotFound();
                        }
                        else
                        {
                            return RedirectToAction(nameof(MyUsers));
                        }
                    }

                    return RedirectToAction(nameof(MyUsers));
                }

                return RedirectToAction(nameof(MyUsers));
            }

            return Unauthorized();

        }


        [HttpPost]
        public IActionResult ToggleBlockUser(UserAdminViewModel viewModel)
        {
            if (User.HasClaim("UserAdmin", "IsUserAdmin") || User.HasClaim("SystemAdmin", "IsSystemAdmin"))
            {
                if (ModelState.IsValid)
                {
                    LaundryUser user;
                    try
                    {
                        user =  _dataAccess.LaundryUsers.GetSingleLaundryUser(viewModel.CurrentLaundryUser.UserName);

                        if (user.LockoutEnd == null)
                        {
                            user.LockoutEnd = new DateTimeOffset(DateTime.MaxValue);
                            user.ActiveUser = false;
                        }
                        else
                        {
                            user.LockoutEnd = null;
                            user.ActiveUser = true;
                        }

                        _dataAccess.Complete();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!_dataAccess.LaundryUsers.LaundryUserExists(viewModel.CurrentLaundryUser.Email))
                        {
                            return NotFound();
                        }

                        TempData["alertMessage"] = "Blocking/Unblocking unsuccessful";
                        return RedirectToAction("EditUser", "UserAdmin", new { email = viewModel.CurrentLaundryUser.Email });
                    }

                    TempData["alertMessage"] = "Blocking/Unblocking successful";
                    return RedirectToAction("EditUser","UserAdmin" ,new {email = user.Email });
                }
                return BadRequest();
            }
            return Unauthorized();
        }

        public IActionResult IndexMachines()
        {
            if (User.HasClaim("UserAdmin", "IsUserAdmin") || User.HasClaim("SystemAdmin", "IsSystemAdmin"))
            {
                var userAdminViewModel = new UserAdminViewModel();
                UserAdmin currentUser = new UserAdmin();
 
                if (User.Identity != null)
                {
                    if(User.HasClaim("SystemAdmin","IsSystemAdmin"))
                    {
                        //currentUser = _dataAccess.UserAdmins.GetSingleUserAdmin(username);
                    }
                    else
                    {
                        currentUser = _dataAccess.UserAdmins.GetSingleUserAdmin(User.Identity.Name);
                    }

                    userAdminViewModel.MyMachines = currentUser.Machines;
                }

                return View(userAdminViewModel);
            }
            
            return Unauthorized();
            
        }

        [HttpGet]
        public IActionResult AddMachines()
        {
            if (User.HasClaim("UserAdmin", "IsUserAdmin") || User.HasClaim("SystemAdmin", "IsSystemAdmin"))
            {
                _userAdminViewModel.CurrentMachine = new Machine();
                return View(_userAdminViewModel);
            }

            return Unauthorized();
        }

        [HttpPost]
        public IActionResult AddMachines(UserAdminViewModel viewModel)
        {
            if (User.HasClaim("UserAdmin", "IsUserAdmin") || User.HasClaim("SystemAdmin", "IsSystemAdmin"))
            {
                if (!ModelState.IsValid)
                {
                    return NotFound();
                }

                if (User.HasClaim("UserAdmin", "IsUserAdmin"))
                    viewModel.CurrentMachine.UserAdmin = _dataAccess.UserAdmins.GetSingleUserAdmin(User.Identity.Name);
                else
                    viewModel.CurrentMachine.UserAdmin = _dataAccess.UserAdmins.GetSingleUserAdmin(viewModel.CurrentUserAdminUserName);

                _dataAccess.Machines.AddMachine(viewModel.CurrentMachine);
                _dataAccess.Complete();

                TempData["Success"] = "true";

                return RedirectToAction(nameof(IndexMachines));
            }

            return Unauthorized();
        }

        [HttpPost]
        public IActionResult DeleteMachines(string MachineToDel)
        {
            if (User.HasClaim("UserAdmin", "IsUserAdmin") || User.HasClaim("SystemAdmin", "IsSystemAdmin"))
            {
                if (!ModelState.IsValid)
                {
                    return NotFound();
                }

                _dataAccess.Machines.DelMachine(int.Parse(MachineToDel));
                _dataAccess.Complete();

                return RedirectToAction(nameof(IndexMachines));
            }

            return Unauthorized();

        }

        [HttpGet]
        public IActionResult GetMessages()
        {
            if (User.Identity != null && User.HasClaim("UserAdmin", "IsUserAdmin"))
            {
                _userAdminViewModel.MyMessages = _dataAccess.MessageList.GetAllMessages();
                _userAdminViewModel.MyMessages.Sort((res1, res2) => res1.SendDate.CompareTo(res2.SendDate));
                _userAdminViewModel.MyMessages.Reverse();

                return View(_userAdminViewModel);
            }
            return Unauthorized();
        }

        public IActionResult DeleteMessage(int msgId)
        {
            if (User.HasClaim("UserAdmin", "IsUserAdmin"))
            {
                if (!ModelState.IsValid)
                {
                    return NotFound();
                }
                _dataAccess.MessageList.DeleteMessage(msgId);
                _dataAccess.Complete();

                return RedirectToAction(nameof(GetMessages));
            }
            return Unauthorized();
        }

        [HttpPost]
        public IActionResult StartMachine(int id)
        {
            if (User.HasClaim("UserAdmin", "IsUserAdmin") || User.HasClaim("SystemAdmin", "IsSystemAdmin"))
            {
                if (!ModelState.IsValid)
                {
                    return NotFound();
                }
                _dataAccess.Machines.StartMachine(id);
                _dataAccess.Complete();

                return RedirectToAction(nameof(IndexMachines));
            }

            return Unauthorized();
        }

        [HttpPost]
        public IActionResult StopMachine(int id)
        {
            if (User.HasClaim("UserAdmin", "IsUserAdmin") || User.HasClaim("SystemAdmin", "IsSystemAdmin"))
            {
                if (!ModelState.IsValid)
                {
                    return NotFound();
                }
                _dataAccess.Machines.StopMachine(id);
                _dataAccess.Complete();

                return RedirectToAction(nameof(IndexMachines));
            }

            return Unauthorized();
        }
    }
}
