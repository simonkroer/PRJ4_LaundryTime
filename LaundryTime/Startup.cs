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
using System.Threading.Tasks;
using LaundryTime.Data.Models;

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

            services.AddDefaultIdentity<SystemAdmin>(options => options.SignIn.RequireConfirmedAccount = true) //Adding SystemAdmin User type
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddDefaultIdentity<UserAdmin>(options => options.SignIn.RequireConfirmedAccount = true) //Adding UserAdmin User type
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddDefaultIdentity<LaundryUser>(options => options.SignIn.RequireConfirmedAccount = true) //Adding LaundryUser User type
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddControllersWithViews();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("IsLaundryUser",
                    policyBuilder => policyBuilder
                        .RequireClaim("LaundryUser"));

                options.AddPolicy("IsAdminUser",
                    policyBuilder => policyBuilder
                        .RequireClaim("AdminUser"));

                options.AddPolicy("IsSystemAdmin",
                    policyBuilder => policyBuilder
                        .RequireClaim("SystemAdmin"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<LaundryUser> userManager1, UserManager<UserAdmin> userManager2, UserManager<SystemAdmin> userManager3, ApplicationDbContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            SeedUsers(userManager1,userManager2,userManager3,context); //Seeding users
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }

        public static void SeedUsers(UserManager<LaundryUser> userManager1, UserManager<UserAdmin> userManager2,
            UserManager<SystemAdmin> userManager3, ApplicationDbContext context)
        {
            ApplicationDbContext _context = context;
            IDataAccessAction dataAcces = new DataAccsessAction(_context);
            const bool emailConfirmed = true;

            //=================== Creating LaundryUser ==========================

            const string laundryUserEmail = "laundryUser@laundryUser.com";
            const string laundryUserPassword = "Sommer25!";
            const string laundryUserCell = "20212223";
            const string laundryUserName = "Jesper Henrik";

            if (userManager1.FindByNameAsync(laundryUserEmail).Result == null)
            {
                var user3 = new LaundryUser();
                user3.UserName = laundryUserEmail;
                user3.Email = laundryUserEmail;
                user3.EmailConfirmed = emailConfirmed;
                user3.PhoneNumber = laundryUserCell;
                user3.Name = laundryUserName;

                IdentityResult result = userManager1.CreateAsync(user3, laundryUserPassword).Result;

            }

            //=================== Creating UserAdmin user ==========================

            const string userAdminEmail = "UserAdmin@UserAdmin.com";
            const string userAdminPassword = "Sommer25!";
            const string userAdminCell = "20212223";
            const string userAdminName = "Knud Knudsen";

            if (userManager2.FindByNameAsync(userAdminEmail).Result == null)
            {
                var user2 = new UserAdmin();
                user2.UserName = userAdminEmail;
                user2.Email = userAdminEmail;
                user2.EmailConfirmed = emailConfirmed;
                user2.PhoneNumber = userAdminCell;
                user2.Name = userAdminName;

                IdentityResult result = userManager2.CreateAsync(user2, userAdminPassword).Result;

                //Adding user to UserAdmin:
                var Useradmin = dataAcces.AdminUsers.GetSingleAdminUser(1);
                Useradmin.Users.Add(dataAcces.LaundryUsers.GetSingleLaundryUser(1));
                context.SaveChanges();

            }


            //==================== Creating System Admin user =======================

            const string systemAdminEmail = "SystemAdmin@LaundryTime.com";
            const string systemAdminPassword = "Sommer25!";
            const string systemAdminCell = "20212223";
            const string systemAdminName = "Kvart Palle";

            if (userManager3.FindByNameAsync(systemAdminEmail).Result == null)
            {
                var user1 = new SystemAdmin();
                user1.UserName = systemAdminEmail;
                user1.Email = systemAdminEmail;
                user1.EmailConfirmed = emailConfirmed;
                user1.PhoneNumber = systemAdminCell;
                user1.Name = systemAdminName;

                IdentityResult result = userManager3.CreateAsync(user1, systemAdminPassword).Result;

                //Adding users to SystemAdmin:
                var systemAdmin = dataAcces.SystemUsers.GetSingleAdminUser(1);
                systemAdmin.LaundryUsers.Add(dataAcces.LaundryUsers.GetSingleLaundryUser(1));
                systemAdmin.UserAdmins.Add(dataAcces.LaundryUsers.GetSingleLaundryUser(1));
                context.SaveChanges();
            }
        }
    }
}
