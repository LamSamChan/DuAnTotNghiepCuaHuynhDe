﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using QuanLyHopDongVaKySo_API.Database;

#nullable disable

namespace QuanLyHopDongVaKySo_API.Migrations
{
    [DbContext(typeof(ProjectDbContext))]
    [Migration("20231012175709_FirstMigration_10.13.23")]
    partial class FirstMigration_101323
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("QuanLyHopDongVaKySo_API.Models.Customer", b =>
                {
                    b.Property<Guid>("CustomerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("Id");

                    b.Property<string>("BankAccount")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("BankName")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("BillingAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("BuisinessName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("Identification")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(50)");

                    b.Property<bool>("IsLocked")
                        .HasColumnType("bit");

                    b.Property<DateTime>("IssuedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("IssuedPlace")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Nationality")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("SerialPFX")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("TOC_ID")
                        .HasColumnType("int");

                    b.Property<string>("TaxIDNumber")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.HasKey("CustomerId");

                    b.HasIndex("SerialPFX");

                    b.HasIndex("TOC_ID");

                    b.ToTable("Customer", (string)null);
                });

            modelBuilder.Entity("QuanLyHopDongVaKySo_API.Models.DoneContract", b =>
                {
                    b.Property<int>("DContractID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DContractID"));

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DConTractName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("DContractFile")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateDone")
                        .HasColumnType("datetime2");

                    b.Property<int>("DoneMinuteId")
                        .HasColumnType("int");

                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsInEffect")
                        .HasColumnType("bit");

                    b.Property<int>("TOS_ID")
                        .HasColumnType("int");

                    b.HasKey("DContractID");

                    b.HasIndex("CustomerId");

                    b.HasIndex("DoneMinuteId");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("TOS_ID");

                    b.ToTable("DoneContract", (string)null);
                });

            modelBuilder.Entity("QuanLyHopDongVaKySo_API.Models.DoneMinute", b =>
                {
                    b.Property<int>("DoneMinuteID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DoneMinuteID"));

                    b.Property<DateTime>("DateDone")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("MinuteFile")
                        .IsRequired()
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("MinuteName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("DoneMinuteID");

                    b.HasIndex("EmployeeId");

                    b.ToTable("DoneMinute");
                });

            modelBuilder.Entity("QuanLyHopDongVaKySo_API.Models.Employee", b =>
                {
                    b.Property<Guid>("EmployeeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("Id");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("Identification")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Image")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<bool>("IsLocked")
                        .HasColumnType("bit");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("PositionID")
                        .HasColumnType("int");

                    b.Property<int>("RoleID")
                        .HasColumnType("int");

                    b.Property<string>("SerialPFX")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("EmployeeId");

                    b.HasIndex("PositionID");

                    b.HasIndex("RoleID");

                    b.HasIndex("SerialPFX");

                    b.ToTable("Employee", (string)null);
                });

            modelBuilder.Entity("QuanLyHopDongVaKySo_API.Models.InstallationRequirement", b =>
                {
                    b.Property<int>("InstallRequireID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("InstallRequireID"));

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<int>("DoneContractId")
                        .HasColumnType("int");

                    b.Property<string>("MinuteFile")
                        .IsRequired()
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("MinuteName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("InstallRequireID");

                    b.HasIndex("DoneContractId");

                    b.ToTable("InstallationRequirement", (string)null);
                });

            modelBuilder.Entity("QuanLyHopDongVaKySo_API.Models.OperationHistory", b =>
                {
                    b.Property<double>("HistoryID")
                        .HasColumnType("float")
                        .HasColumnName("Id");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("OperationName")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("HistoryID");

                    b.ToTable("OperationHistory", (string)null);
                });

            modelBuilder.Entity("QuanLyHopDongVaKySo_API.Models.PFXCertificate", b =>
                {
                    b.Property<string>("Serial")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("Id");

                    b.Property<string>("ImageSignature1")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("ImageSignature2")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("ImageSignature3")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<bool>("IsEmployee")
                        .HasColumnType("bit");

                    b.Property<string>("Issuer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Issuer");

                    b.Property<string>("PfxFileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Name");

                    b.Property<string>("PfxPassword")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Password");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Subject");

                    b.Property<string>("Thumbprint")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Thumbprint");

                    b.Property<DateTime>("ValidFrom")
                        .HasColumnType("datetime2")
                        .HasColumnName("ValidFrom");

                    b.Property<DateTime>("ValidUntil")
                        .HasColumnType("datetime2")
                        .HasColumnName("ValidUntil");

                    b.HasKey("Serial");

                    b.ToTable("PFXCertificate", (string)null);
                });

            modelBuilder.Entity("QuanLyHopDongVaKySo_API.Models.PendingContract", b =>
                {
                    b.Property<int>("PContractID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PContractID"));

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsCustomer")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDirector")
                        .HasColumnType("bit");

                    b.Property<bool>("IsRefuse")
                        .HasColumnType("bit");

                    b.Property<string>("PContractFile")
                        .IsRequired()
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("PContractName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Reason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TContractId")
                        .HasColumnType("int");

                    b.Property<int>("TOS_ID")
                        .HasColumnType("int");

                    b.HasKey("PContractID");

                    b.HasIndex("CustomerId");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("TContractId");

                    b.HasIndex("TOS_ID");

                    b.ToTable("PendingContract", (string)null);
                });

            modelBuilder.Entity("QuanLyHopDongVaKySo_API.Models.PendingMinute", b =>
                {
                    b.Property<int>("InstallationMinuteID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("InstallationMinuteID"));

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<int>("DoneContractId")
                        .HasColumnType("int");

                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsCustomer")
                        .HasColumnType("bit");

                    b.Property<bool>("IsIntallation")
                        .HasColumnType("bit");

                    b.Property<string>("MinuteFile")
                        .IsRequired()
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("MinuteName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("TMinuteId")
                        .HasColumnType("int");

                    b.HasKey("InstallationMinuteID");

                    b.HasIndex("DoneContractId");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("TMinuteId");

                    b.ToTable("InstallationMinute", (string)null);
                });

            modelBuilder.Entity("QuanLyHopDongVaKySo_API.Models.Position", b =>
                {
                    b.Property<int>("PositionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PositionID"));

                    b.Property<string>("PositionName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("PositionID");

                    b.ToTable("Position", (string)null);
                });

            modelBuilder.Entity("QuanLyHopDongVaKySo_API.Models.Role", b =>
                {
                    b.Property<int>("RoleID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoleID"));

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("RoleID");

                    b.ToTable("Role", (string)null);
                });

            modelBuilder.Entity("QuanLyHopDongVaKySo_API.Models.TemplateContract", b =>
                {
                    b.Property<int>("TContactID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TContactID"));

                    b.Property<DateTime>("DateAdded")
                        .HasColumnType("datetime2");

                    b.Property<string>("TContractFile")
                        .IsRequired()
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("TContractName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("x_CustomerZone")
                        .HasColumnType("int");

                    b.Property<int>("x_DirectorZone")
                        .HasColumnType("int");

                    b.Property<int>("y_CustomerZone")
                        .HasColumnType("int");

                    b.Property<int>("y_DirectorZone")
                        .HasColumnType("int");

                    b.HasKey("TContactID");

                    b.ToTable("TemplateContract", (string)null);
                });

            modelBuilder.Entity("QuanLyHopDongVaKySo_API.Models.TemplateMinute", b =>
                {
                    b.Property<int>("TMinuteID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TMinuteID"));

                    b.Property<DateTime>("DateAdded")
                        .HasColumnType("datetime2");

                    b.Property<string>("TMinuteFile")
                        .IsRequired()
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("TMinuteName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("x_CustomerZone")
                        .HasColumnType("int");

                    b.Property<int>("x_IntallationZone")
                        .HasColumnType("int");

                    b.Property<int>("y_CustomerZone")
                        .HasColumnType("int");

                    b.Property<int>("y_IntallationZone")
                        .HasColumnType("int");

                    b.HasKey("TMinuteID");

                    b.ToTable("TemplateMinute", (string)null);
                });

            modelBuilder.Entity("QuanLyHopDongVaKySo_API.Models.TypeOfCustomer", b =>
                {
                    b.Property<int>("TOC_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TOC_ID"));

                    b.Property<DateTime>("DateAdded")
                        .HasColumnType("datetime2");

                    b.Property<string>("TypeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TOC_ID");

                    b.ToTable("TypeOfCustomer", (string)null);
                });

            modelBuilder.Entity("QuanLyHopDongVaKySo_API.Models.TypeOfService", b =>
                {
                    b.Property<int>("TOS_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TOS_ID"));

                    b.Property<DateTime>("DateAdded")
                        .HasColumnType("datetime2");

                    b.Property<string>("PerTime")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("ServiceName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("TOS_ID");

                    b.ToTable("TypeOfService", (string)null);
                });

            modelBuilder.Entity("QuanLyHopDongVaKySo_API.Models.Customer", b =>
                {
                    b.HasOne("QuanLyHopDongVaKySo_API.Models.PFXCertificate", "PFXCertificate")
                        .WithMany()
                        .HasForeignKey("SerialPFX")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QuanLyHopDongVaKySo_API.Models.TypeOfCustomer", "TypeOfCustomer")
                        .WithMany()
                        .HasForeignKey("TOC_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PFXCertificate");

                    b.Navigation("TypeOfCustomer");
                });

            modelBuilder.Entity("QuanLyHopDongVaKySo_API.Models.DoneContract", b =>
                {
                    b.HasOne("QuanLyHopDongVaKySo_API.Models.Customer", "Customer")
                        .WithMany("DoneContract")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QuanLyHopDongVaKySo_API.Models.DoneMinute", "DoneMinute")
                        .WithMany()
                        .HasForeignKey("DoneMinuteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QuanLyHopDongVaKySo_API.Models.Employee", "Employee")
                        .WithMany("DoneContract")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QuanLyHopDongVaKySo_API.Models.TypeOfService", "TypeOfService")
                        .WithMany()
                        .HasForeignKey("TOS_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("DoneMinute");

                    b.Navigation("Employee");

                    b.Navigation("TypeOfService");
                });

            modelBuilder.Entity("QuanLyHopDongVaKySo_API.Models.DoneMinute", b =>
                {
                    b.HasOne("QuanLyHopDongVaKySo_API.Models.Employee", "Employee")
                        .WithMany("DoneMinute")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("QuanLyHopDongVaKySo_API.Models.Employee", b =>
                {
                    b.HasOne("QuanLyHopDongVaKySo_API.Models.Position", "Position")
                        .WithMany()
                        .HasForeignKey("PositionID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QuanLyHopDongVaKySo_API.Models.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QuanLyHopDongVaKySo_API.Models.PFXCertificate", "PFXCertificate")
                        .WithMany()
                        .HasForeignKey("SerialPFX")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PFXCertificate");

                    b.Navigation("Position");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("QuanLyHopDongVaKySo_API.Models.InstallationRequirement", b =>
                {
                    b.HasOne("QuanLyHopDongVaKySo_API.Models.DoneContract", "DoneContract")
                        .WithMany()
                        .HasForeignKey("DoneContractId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DoneContract");
                });

            modelBuilder.Entity("QuanLyHopDongVaKySo_API.Models.PendingContract", b =>
                {
                    b.HasOne("QuanLyHopDongVaKySo_API.Models.Customer", "Customer")
                        .WithMany("PendingContract")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QuanLyHopDongVaKySo_API.Models.Employee", "Employee")
                        .WithMany("PendingContract")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QuanLyHopDongVaKySo_API.Models.TemplateContract", "TemplateContract")
                        .WithMany()
                        .HasForeignKey("TContractId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QuanLyHopDongVaKySo_API.Models.TypeOfService", "TypeOfService")
                        .WithMany()
                        .HasForeignKey("TOS_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Employee");

                    b.Navigation("TemplateContract");

                    b.Navigation("TypeOfService");
                });

            modelBuilder.Entity("QuanLyHopDongVaKySo_API.Models.PendingMinute", b =>
                {
                    b.HasOne("QuanLyHopDongVaKySo_API.Models.DoneContract", "DoneContract")
                        .WithMany()
                        .HasForeignKey("DoneContractId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QuanLyHopDongVaKySo_API.Models.Employee", "Employee")
                        .WithMany("PendingMinute")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QuanLyHopDongVaKySo_API.Models.TemplateMinute", "TemplateMinute")
                        .WithMany()
                        .HasForeignKey("TMinuteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DoneContract");

                    b.Navigation("Employee");

                    b.Navigation("TemplateMinute");
                });

            modelBuilder.Entity("QuanLyHopDongVaKySo_API.Models.Customer", b =>
                {
                    b.Navigation("DoneContract");

                    b.Navigation("PendingContract");
                });

            modelBuilder.Entity("QuanLyHopDongVaKySo_API.Models.Employee", b =>
                {
                    b.Navigation("DoneContract");

                    b.Navigation("DoneMinute");

                    b.Navigation("PendingContract");

                    b.Navigation("PendingMinute");
                });
#pragma warning restore 612, 618
        }
    }
}
