﻿// <auto-generated />
using System;
using LaundryTime.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LaundryTime.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.5")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("LaundryTime.Data.Models.Address", b =>
                {
                    b.Property<int>("AddressId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StreetAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Zipcode")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AddressId");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("LaundryTime.Data.Models.Booking.BookingListModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int?>("DateModelId")
                        .HasColumnType("int");

                    b.Property<int>("MachineId")
                        .HasColumnType("int");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<string>("Time")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DateModelId");

                    b.HasIndex("MachineId");

                    b.ToTable("BookingListModels");
                });

            modelBuilder.Entity("LaundryTime.Data.Models.Booking.DateModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateData")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("DateModels");
                });

            modelBuilder.Entity("LaundryTime.Data.Models.Booking.ReservedListModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int?>("DateModelId")
                        .HasColumnType("int");

                    b.Property<int>("MachineId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OldId")
                        .HasColumnType("int");

                    b.Property<string>("Time")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DateModelId");

                    b.HasIndex("MachineId");

                    b.ToTable("ReservedListModels");
                });

            modelBuilder.Entity("LaundryTime.Data.Models.LaundryLog", b =>
                {
                    b.Property<string>("LogId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("LogDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("LogInfo")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LogId");

                    b.ToTable("LaundryLog");
                });

            modelBuilder.Entity("LaundryTime.Data.Models.Machine", b =>
                {
                    b.Property<int>("MachineId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("InstallationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModelNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserAdminId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("MachineId");

                    b.HasIndex("UserAdminId");

                    b.ToTable("Machines");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityUser");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("LaundryTime.Data.Models.ApplicationUser", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.IdentityUser");

                    b.HasDiscriminator().HasValue("ApplicationUser");
                });

            modelBuilder.Entity("LaundryTime.Data.Models.LaundryUser", b =>
                {
                    b.HasBaseType("LaundryTime.Data.Models.ApplicationUser");

                    b.Property<bool>("ActiveUser")
                        .HasColumnType("bit");

                    b.Property<int?>("AddressId")
                        .HasColumnType("int");

                    b.Property<string>("AdministratorId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal>("FinancialBalance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("PaymentDueDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PaymentMethod")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SystemAdminId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserAdminId")
                        .HasColumnType("nvarchar(450)");

                    b.HasIndex("AddressId");

                    b.HasIndex("AdministratorId");

                    b.HasIndex("SystemAdminId");

                    b.HasIndex("UserAdminId");

                    b.HasDiscriminator().HasValue("LaundryUser");
                });

            modelBuilder.Entity("LaundryTime.Data.Models.SystemAdmin", b =>
                {
                    b.HasBaseType("LaundryTime.Data.Models.ApplicationUser");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("SystemAdmin_Name");

                    b.HasDiscriminator().HasValue("SystemAdmin");
                });

            modelBuilder.Entity("LaundryTime.Data.Models.UserAdmin", b =>
                {
                    b.HasBaseType("LaundryTime.Data.Models.ApplicationUser");

                    b.Property<int?>("AddressId")
                        .HasColumnType("int")
                        .HasColumnName("UserAdmin_AddressId");

                    b.Property<decimal>("FinancialBalance")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("UserAdmin_FinancialBalance");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("UserAdmin_Name");

                    b.Property<DateTime>("PaymentDueDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("UserAdmin_PaymentDueDate");

                    b.Property<string>("PaymentMethod")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("UserAdmin_PaymentMethod");

                    b.Property<string>("SystemAdminId")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("UserAdmin_SystemAdminId");

                    b.HasIndex("AddressId");

                    b.HasIndex("SystemAdminId");

                    b.HasDiscriminator().HasValue("UserAdmin");
                });

            modelBuilder.Entity("LaundryTime.Data.Models.Booking.BookingListModel", b =>
                {
                    b.HasOne("LaundryTime.Data.Models.Booking.DateModel", "DateModel")
                        .WithMany("BookingListModels")
                        .HasForeignKey("DateModelId");

                    b.HasOne("LaundryTime.Data.Models.Machine", "Machine")
                        .WithMany()
                        .HasForeignKey("MachineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DateModel");

                    b.Navigation("Machine");
                });

            modelBuilder.Entity("LaundryTime.Data.Models.Booking.ReservedListModel", b =>
                {
                    b.HasOne("LaundryTime.Data.Models.Booking.DateModel", null)
                        .WithMany("ReservedListModels")
                        .HasForeignKey("DateModelId");

                    b.HasOne("LaundryTime.Data.Models.Machine", "Machine")
                        .WithMany()
                        .HasForeignKey("MachineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Machine");
                });

            modelBuilder.Entity("LaundryTime.Data.Models.LaundryLog", b =>
                {
                    b.HasOne("LaundryTime.Data.Models.LaundryUser", null)
                        .WithMany("LaundryHistory")
                        .HasForeignKey("LogId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("LaundryTime.Data.Models.Machine", b =>
                {
                    b.HasOne("LaundryTime.Data.Models.UserAdmin", "UserAdmin")
                        .WithMany("Machines")
                        .HasForeignKey("UserAdminId");

                    b.Navigation("UserAdmin");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("LaundryTime.Data.Models.LaundryUser", b =>
                {
                    b.HasOne("LaundryTime.Data.Models.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId");

                    b.HasOne("LaundryTime.Data.Models.UserAdmin", "Administrator")
                        .WithMany()
                        .HasForeignKey("AdministratorId");

                    b.HasOne("LaundryTime.Data.Models.SystemAdmin", null)
                        .WithMany("LaundryUsers")
                        .HasForeignKey("SystemAdminId");

                    b.HasOne("LaundryTime.Data.Models.UserAdmin", null)
                        .WithMany("Users")
                        .HasForeignKey("UserAdminId");

                    b.Navigation("Address");

                    b.Navigation("Administrator");
                });

            modelBuilder.Entity("LaundryTime.Data.Models.UserAdmin", b =>
                {
                    b.HasOne("LaundryTime.Data.Models.Address", "WorkAddress")
                        .WithMany()
                        .HasForeignKey("AddressId");

                    b.HasOne("LaundryTime.Data.Models.SystemAdmin", null)
                        .WithMany("UserAdmins")
                        .HasForeignKey("SystemAdminId");

                    b.Navigation("WorkAddress");
                });

            modelBuilder.Entity("LaundryTime.Data.Models.Booking.DateModel", b =>
                {
                    b.Navigation("BookingListModels");

                    b.Navigation("ReservedListModels");
                });

            modelBuilder.Entity("LaundryTime.Data.Models.LaundryUser", b =>
                {
                    b.Navigation("LaundryHistory");
                });

            modelBuilder.Entity("LaundryTime.Data.Models.SystemAdmin", b =>
                {
                    b.Navigation("LaundryUsers");

                    b.Navigation("UserAdmins");
                });

            modelBuilder.Entity("LaundryTime.Data.Models.UserAdmin", b =>
                {
                    b.Navigation("Machines");

                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
