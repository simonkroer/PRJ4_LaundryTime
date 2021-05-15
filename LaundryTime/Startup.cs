using LaundryTime.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using LaundryTime.Data.Models;
using LaundryTime.Data.Models.Booking;
using MailKit.Net.Smtp;
using MimeKit;

namespace LaundryTime
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("ThomasConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            var notificationMetadata =
                Configuration.GetSection("NotificationMetadata").
                    Get<NotificationMetadata>();
            services.AddSingleton(notificationMetadata);
            services.AddControllers();

            services
                .AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true) //Adding LaundryUser User type
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddControllersWithViews();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("IsLaundryUser",
                    policyBuilder => policyBuilder
                        .RequireClaim("LaundryUser"));

                options.AddPolicy("IsUserAdmin",
                    policyBuilder => policyBuilder
                        .RequireClaim("AdminUser"));

                options.AddPolicy("IsSystemAdmin",
                    policyBuilder => policyBuilder
                        .RequireClaim("SystemAdmin"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            //
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //    app.UseHsts();
            //}
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            SeedUsers(userManager, context); //Seeding users
            SeedMachines(context); //Seeding Machinces

            DateTime date = DateTime.Parse("22-04-2021");
            var check = context.DateModels.FirstOrDefault(c => c.DateData == date);
            if (check == null)
            {
                CreateNewBookList(context, CreateDateModel(context, "22-04-2021"));
            }
            date = DateTime.Parse("23-04-2021");
            check = context.DateModels.FirstOrDefault(c => c.DateData == date);
            if (check == null)
            {
                CreateNewBookList(context, CreateDateModel(context, "23-04-2021"));
            }
            

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }

        public static void SeedUsers(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            ApplicationDbContext _context = context;
            IDataAccessAction dataAcces = new DataAccsessAction(_context);
            const bool emailConfirmed = true;

            //=================== Creating LaundryUsers ==========================

            const string laundryUserEmail = "laundryUser@laundryUser.com";
            const string laundryUserPassword = "Sommer25!";
            const string laundryUserCell = "20212223";
            const string laundryUserName = "Jesper Henrik";
            const string laundryUserPayment = "MobilePay";
            const bool active = true;

            if (!dataAcces.LaundryUsers.LaundryUserExists(laundryUserEmail))
            {
                var user3 = new LaundryUser();
                user3.UserName = laundryUserEmail;
                user3.Email = laundryUserEmail;
                user3.EmailConfirmed = emailConfirmed;
                user3.PhoneNumber = laundryUserCell;
                user3.Name = laundryUserName;
                user3.PaymentMethod = laundryUserPayment;
                user3.ActiveUser = active;

                IdentityResult result = userManager.CreateAsync(user3, laundryUserPassword).Result;

                if (result.Succeeded) //Add claim to user
                {
                    userManager.AddClaimAsync(user3, new Claim("LaundryUser", "IsLaundryUser")).Wait();
                }

            }
            const string laundryUser2Email = "laundryUser2@laundryUser.com";
            const string laundryUser2Password = "Sommer25!";
            const string laundryUser2Cell = "88888888";
            const string laundryUser2Name = "Dave Jensens";
            const string laundryUser2Payment = "MobilePay";

            if (!dataAcces.LaundryUsers.LaundryUserExists(laundryUser2Email))
            {
                var user4 = new LaundryUser();
                user4.UserName = laundryUser2Email;
                user4.Email = laundryUser2Email;
                user4.EmailConfirmed = emailConfirmed;
                user4.PhoneNumber = laundryUser2Cell;
                user4.Name = laundryUser2Name;
                user4.PaymentMethod = laundryUser2Payment;
                user4.ActiveUser = active;

                IdentityResult result = userManager.CreateAsync(user4, laundryUser2Password).Result;

                if (result.Succeeded) //Add claim to user
                {
                    userManager.AddClaimAsync(user4, new Claim("LaundryUser", "IsLaundryUser")).Wait();
                }

            }

            //=================== Creating LaundryLog ==========================
            const string infoForLog = "This is a damn test";
            DateTime logTime = DateTime.Now;
            const string infoForLog2 = "This is a damn test too";
            DateTime logTime2 = DateTime.Now;
            var user3ForLog = dataAcces.LaundryUsers.GetSingleLaundryUser("laundryUser@laundryUser.com"); 
            if(user3ForLog !=null)
            {
                if (user3ForLog.LaundryHistory.Count() == 0)
                {
                    var user3Log = new LaundryLog();
                    user3Log.LogDate = logTime;
                    user3Log.LogInfo = infoForLog;
                    user3ForLog.LaundryHistory.Add(user3Log);
                    context.SaveChanges();
                }
            }

            //var user3Log2 = new LaundryLog();
            //user3Log2.LogDate = logTime2;
            //user3Log2.LogInfo = infoForLog2;

            //user3ForLog.LaundryHistory.Add(user3Log2);
            //dataAcces.Complete();

            //=================== Creating UserAdmin user ==========================

            const string userAdminEmail = "UserAdmin@UserAdmin.com";
            const string userAdminPassword = "Sommer25!";
            const string userAdminCell = "20212223";
            const string userAdminName = "Knud Knudsen";
            const string userAdminPayment = "MobilePay";

            if (!dataAcces.UserAdmins.UserExists(userAdminEmail))
            {
                var user2 = new UserAdmin();
                user2.UserName = userAdminEmail;
                user2.Email = userAdminEmail;
                user2.EmailConfirmed = emailConfirmed;
                user2.PhoneNumber = userAdminCell;
                user2.Name = userAdminName;
                user2.PaymentMethod = userAdminPayment;

                IdentityResult result = userManager.CreateAsync(user2, userAdminPassword).Result;

                if (result.Succeeded) //Add claim to user
                {
                    userManager.AddClaimAsync(user2, new Claim("UserAdmin", "IsUserAdmin")).Wait();
                }

            }

            var useradmin = dataAcces.UserAdmins.GetSingleUserAdmin(userAdminEmail);

            if (useradmin!=null)
            {
                //Adding user to UserAdmin:
                if (useradmin.Users != null)
                    useradmin.Users.Add(dataAcces.LaundryUsers.GetSingleLaundryUser(laundryUserEmail));
                dataAcces.Complete();
            }


            //==================== Creating System Admin user =======================

            const string systemAdminEmail = "SystemAdmin@LaundryTime.com";
            const string systemAdminPassword = "Sommer25!";
            const string systemAdminCell = "20212223";
            const string systemAdminName = "Kvart Palle";

            if (!dataAcces.SystemAdmins.UserExists(systemAdminEmail))
            {
                var user1 = new SystemAdmin();
                user1.UserName = systemAdminEmail;
                user1.Email = systemAdminEmail;
                user1.EmailConfirmed = emailConfirmed;
                user1.PhoneNumber = systemAdminCell;
                user1.Name = systemAdminName;

                IdentityResult result = userManager.CreateAsync(user1, systemAdminPassword).Result;

                if (result.Succeeded) //Add claim to user
                {
                    userManager.AddClaimAsync(user1, new Claim("SystemAdmin", "IsSystemAdmin")).Wait();
                }

                //Adding users to SystemAdmin:
                var systemAdmin = dataAcces.SystemAdmins.GetSingleSystemAdmin(systemAdminEmail);
                systemAdmin.LaundryUsers.Add(dataAcces.LaundryUsers.GetSingleLaundryUser(laundryUserEmail));
                systemAdmin.UserAdmins.Add(dataAcces.UserAdmins.GetSingleUserAdmin(userAdminEmail));
                dataAcces.Complete();
            }
        }


        public static void SeedMachines(ApplicationDbContext context)
        {
            ApplicationDbContext _context = context;
            IDataAccessAction dataAcces = new DataAccsessAction(_context);
            const string userAdminEmail = "UserAdmin@UserAdmin.com";
            //==================== Creating Machines =======================

            string ModelNumber = "SE-59-574W";

            if (!dataAcces.Machines.MachineExist(ModelNumber))
            {
                var machine = new Machine();
                machine.Type = "Washing";
                machine.InstallationDate = DateTime.Today;
                machine.ModelNumber = ModelNumber;

                //Adding machine to DB:
                var useradmin = dataAcces.UserAdmins.GetSingleUserAdmin(userAdminEmail);
                useradmin.Machines.Add(machine);
                context.SaveChanges();
            }

            ModelNumber = "SE-59-355W";

            if (!dataAcces.Machines.MachineExist(ModelNumber))
            {
                var machine = new Machine();
                machine.Type = "Washing";
                machine.InstallationDate = DateTime.Today;
                machine.ModelNumber = ModelNumber;

                //Adding machine to DB:
                var useradmin = dataAcces.UserAdmins.GetSingleUserAdmin(userAdminEmail);
                useradmin.Machines.Add(machine);
                dataAcces.Complete();
            }

            ModelNumber = "SE-59-238W";

            if (!dataAcces.Machines.MachineExist(ModelNumber))
            {
                var machine = new Machine();
                machine.Type = "Washing";
                machine.InstallationDate = DateTime.Today;
                machine.ModelNumber = ModelNumber;

                //Adding machine to DB:
                var useradmin = dataAcces.UserAdmins.GetSingleUserAdmin(userAdminEmail);
                useradmin.Machines.Add(machine);
                dataAcces.Complete();
            }

            ModelNumber = "SE-33-245D";

            if (!dataAcces.Machines.MachineExist(ModelNumber))
            {
                var machine = new Machine();
                machine.Type = "Drying";
                machine.InstallationDate = DateTime.Today;
                machine.ModelNumber = ModelNumber;

                //Adding machine to DB:
                var useradmin = dataAcces.UserAdmins.GetSingleUserAdmin(userAdminEmail);
                useradmin.Machines.Add(machine);
                dataAcces.Complete();
            }

            ModelNumber = "SE-33-650D";

            if (!dataAcces.Machines.MachineExist(ModelNumber))
            {
                var machine = new Machine();
                machine.Type = "Drying";
                machine.InstallationDate = DateTime.Today;
                machine.ModelNumber = ModelNumber;

                //Adding machine to DB:
                var useradmin = dataAcces.UserAdmins.GetSingleUserAdmin(userAdminEmail);
                useradmin.Machines.Add(machine);
                dataAcces.Complete();
            }
        }

        public static void CreateNewBookList(ApplicationDbContext context, DateModel date)
        {
            ApplicationDbContext _context = context;
            IDataAccessAction dataAcces = new DataAccsessAction(_context);
            BookingListModel[] BooklistM1tmp = new BookingListModel[15];

            var machines = dataAcces.Machines.GetAllMachines();
            foreach (var machine in machines)
            {
                for (int i = 8; i < 23; i++)
                {
                    int t = i - 8;
                    string time = i.ToString() + "-" + (i + 1).ToString();
                    BooklistM1tmp[t] = new BookingListModel()
                    {
                        DateModel = date,
                        Date = date.DateData,
                        Status = true,
                        Machine = machine,
                        Time = time
                    };
                    dataAcces.BookingList.Add(BooklistM1tmp[t]);
                }
            }
            
            dataAcces.Complete();
        }
        public DateModel CreateDateModel(ApplicationDbContext context, string dato)
        {
            ApplicationDbContext _context = context;
            IDataAccessAction dataAcces = new DataAccsessAction(_context);
            //DatePickerModel date = new DatePickerModel();
            DateTime date = DateTime.Parse(dato);
            DateModel newDateModel = new DateModel()
            {
                DateData = date.Date
            };

            _context.DateModels.Add(newDateModel);
            dataAcces.Complete();

            return newDateModel;
        }

    }
}


