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
    [Migration("20231023190221_102423Migration")]
    partial class _102423Migration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("QuanLyHopDongVaKySo_API.Models.ContractCoordinate", b =>
                {
                    b.Property<int>("ContractCoorID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ContractCoorID"));

                    b.Property<string>("FieldName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("SignaturePage")
                        .HasColumnType("int");

                    b.Property<int>("TContractID")
                        .HasColumnType("int");

                    b.Property<float>("X")
                        .HasColumnType("real");

                    b.Property<float>("Y")
                        .HasColumnType("real");

                    b.HasKey("ContractCoorID");

                    b.HasIndex("TContractID");

                    b.ToTable("ContractCoordinate", (string)null);
                });

            modelBuilder.Entity("QuanLyHopDongVaKySo_API.Models.Customer", b =>
                {
                    b.Property<Guid?>("CustomerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("Id");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("BankAccount")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("BankName")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("BuisinessName")
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
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
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Nationality")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Position")
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("SerialPFX")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("TOC_ID")
                        .HasColumnType("int");

                    b.Property<string>("TaxIDNumber")
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

                    b.Property<Guid?>("EmployeeId")
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
                    b.Property<Guid?>("EmployeeId")
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
                        .HasMaxLength(50)
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

                    b.Property<bool>("IsFirstLogin")
                        .HasColumnType("bit");

                    b.Property<bool>("IsLocked")
                        .HasColumnType("bit");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Password")
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

                    b.Property<int>("TMinuteId")
                        .HasColumnType("int");

                    b.HasKey("InstallRequireID");

                    b.HasIndex("DoneContractId");

                    b.HasIndex("TMinuteId");

                    b.ToTable("InstallationRequirement", (string)null);
                });

            modelBuilder.Entity("QuanLyHopDongVaKySo_API.Models.MinuteCoordinate", b =>
                {
                    b.Property<int>("MinuteCoorID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MinuteCoorID"));

                    b.Property<string>("FieldName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("SignaturePage")
                        .HasColumnType("int");

                    b.Property<int>("TMinuteID")
                        .HasColumnType("int");

                    b.Property<float>("X")
                        .HasColumnType("real");

                    b.Property<float>("Y")
                        .HasColumnType("real");

                    b.HasKey("MinuteCoorID");

                    b.HasIndex("TMinuteID");

                    b.ToTable("MinuteCoordinate", (string)null);
                });

            modelBuilder.Entity("QuanLyHopDongVaKySo_API.Models.OperationHistoryCus", b =>
                {
                    b.Property<int>("HistoryID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("HistoryID"));

                    b.Property<Guid>("CustomerID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("OperationName")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("HistoryID");

                    b.HasIndex("CustomerID");

                    b.ToTable("OperationHistoryCus", (string)null);
                });

            modelBuilder.Entity("QuanLyHopDongVaKySo_API.Models.OperationHistoryEmp", b =>
                {
                    b.Property<int>("HistoryID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("HistoryID"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("EmployeeID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("OperationName")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("HistoryID");

                    b.HasIndex("EmployeeID");

                    b.ToTable("OperationHistoryEmp", (string)null);
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

                    b.Property<string>("ImageSignature4")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("ImageSignature5")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<bool>("IsEmployee")
                        .HasColumnType("bit");

                    b.Property<string>("Issuer")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Issuer");

                    b.Property<string>("PfxFileName")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Name");

                    b.Property<string>("PfxFilePath")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("FilePath");

                    b.Property<string>("PfxPassword")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Password");

                    b.Property<string>("Subject")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Subject");

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
                    b.Property<int>("PendingMinuteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PendingMinuteId"));

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

                    b.HasKey("PendingMinuteId");

                    b.HasIndex("DoneContractId");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("TMinuteId");

                    b.ToTable("PendingMinute", (string)null);
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

                    b.Property<bool>("isHidden")
                        .HasColumnType("bit");

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

                    b.Property<bool>("isHidden")
                        .HasColumnType("bit");

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

                    b.Property<string>("jsonCustomerZone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("jsonDirectorZone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

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

                    b.Property<string>("jsonCustomerZone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("jsonIntallationZone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

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

                    b.Property<string>("TypeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("isHidden")
                        .HasColumnType("bit");

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

                    b.Property<DateTime?>("DateAdded")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("PerTime")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("ServiceName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("isHidden")
                        .HasColumnType("bit");

                    b.HasKey("TOS_ID");

                    b.ToTable("TypeOfService", (string)null);
                });

            modelBuilder.Entity("QuanLyHopDongVaKySo_API.Models.ContractCoordinate", b =>
                {
                    b.HasOne("QuanLyHopDongVaKySo_API.Models.TemplateContract", "TemplateContract")
                        .WithMany("ContractCoordinates")
                        .HasForeignKey("TContractID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TemplateContract");
                });

            modelBuilder.Entity("QuanLyHopDongVaKySo_API.Models.Customer", b =>
                {
                    b.HasOne("QuanLyHopDongVaKySo_API.Models.PFXCertificate", "PFXCertificate")
                        .WithMany()
                        .HasForeignKey("SerialPFX");

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
                        .HasForeignKey("EmployeeId");

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
                        .HasForeignKey("SerialPFX");

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

                    b.HasOne("QuanLyHopDongVaKySo_API.Models.TemplateMinute", "TemplateMinute")
                        .WithMany()
                        .HasForeignKey("TMinuteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DoneContract");

                    b.Navigation("TemplateMinute");
                });

            modelBuilder.Entity("QuanLyHopDongVaKySo_API.Models.MinuteCoordinate", b =>
                {
                    b.HasOne("QuanLyHopDongVaKySo_API.Models.TemplateMinute", "TemplateMinute")
                        .WithMany("MinuteCoordinates")
                        .HasForeignKey("TMinuteID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TemplateMinute");
                });

            modelBuilder.Entity("QuanLyHopDongVaKySo_API.Models.OperationHistoryCus", b =>
                {
                    b.HasOne("QuanLyHopDongVaKySo_API.Models.Customer", "Customer")
                        .WithMany("OperationHistoryCus")
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("QuanLyHopDongVaKySo_API.Models.OperationHistoryEmp", b =>
                {
                    b.HasOne("QuanLyHopDongVaKySo_API.Models.Employee", "Employee")
                        .WithMany("OperationHistoryEmp")
                        .HasForeignKey("EmployeeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
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

                    b.Navigation("OperationHistoryCus");

                    b.Navigation("PendingContract");
                });

            modelBuilder.Entity("QuanLyHopDongVaKySo_API.Models.Employee", b =>
                {
                    b.Navigation("DoneContract");

                    b.Navigation("DoneMinute");

                    b.Navigation("OperationHistoryEmp");

                    b.Navigation("PendingContract");

                    b.Navigation("PendingMinute");
                });

            modelBuilder.Entity("QuanLyHopDongVaKySo_API.Models.TemplateContract", b =>
                {
                    b.Navigation("ContractCoordinates");
                });

            modelBuilder.Entity("QuanLyHopDongVaKySo_API.Models.TemplateMinute", b =>
                {
                    b.Navigation("MinuteCoordinates");
                });
#pragma warning restore 612, 618
        }
    }
}
