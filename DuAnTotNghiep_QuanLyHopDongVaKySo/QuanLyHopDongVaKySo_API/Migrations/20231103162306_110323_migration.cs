using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyHopDongVaKySo_API.Migrations
{
    /// <inheritdoc />
    public partial class _110323_migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PFXCertificate",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Issuer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValidFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidUntil = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsEmployee = table.Column<bool>(type: "bit", nullable: false),
                    ImageSignature1 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ImageSignature2 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ImageSignature3 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ImageSignature4 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ImageSignature5 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PFXCertificate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Position",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PositionName = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    isHidden = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Position", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    isHidden = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TemplateContract",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TContractName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    TContractFile = table.Column<string>(type: "nvarchar(250)", nullable: false),
                    jsonDirectorZone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    jsonCustomerZone = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateContract", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TemplateMinute",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TMinuteName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    TMinuteFile = table.Column<string>(type: "nvarchar(250)", nullable: false),
                    jsonIntallationZone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    jsonCustomerZone = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateMinute", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TypeOfCustomer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isHidden = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeOfCustomer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Email = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Identification = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Password = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    IsFirstLogin = table.Column<bool>(type: "bit", nullable: false),
                    SerialPFX = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RoleID = table.Column<int>(type: "int", nullable: false),
                    PositionID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employee_PFXCertificate_SerialPFX",
                        column: x => x.SerialPFX,
                        principalTable: "PFXCertificate",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Employee_Position_PositionID",
                        column: x => x.PositionID,
                        principalTable: "Position",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employee_Role_RoleID",
                        column: x => x.RoleID,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContractCoordinate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FieldName = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    X = table.Column<float>(type: "real", nullable: false),
                    Y = table.Column<float>(type: "real", nullable: false),
                    SignaturePage = table.Column<int>(type: "int", nullable: false),
                    TContractID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractCoordinate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContractCoordinate_TemplateContract_TContractID",
                        column: x => x.TContractID,
                        principalTable: "TemplateContract",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MinuteCoordinate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FieldName = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    X = table.Column<float>(type: "real", nullable: false),
                    Y = table.Column<float>(type: "real", nullable: false),
                    SignaturePage = table.Column<int>(type: "int", nullable: false),
                    TMinuteID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MinuteCoordinate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MinuteCoordinate_TemplateMinute_TMinuteID",
                        column: x => x.TMinuteID,
                        principalTable: "TemplateMinute",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TypeOfService",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ServiceName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PerTime = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    isHidden = table.Column<bool>(type: "bit", nullable: false),
                    templateContractID = table.Column<int>(type: "int", nullable: false),
                    templateMinuteID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeOfService", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TypeOfService_TemplateContract_templateContractID",
                        column: x => x.templateContractID,
                        principalTable: "TemplateContract",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TypeOfService_TemplateMinute_templateMinuteID",
                        column: x => x.templateMinuteID,
                        principalTable: "TemplateMinute",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BuisinessName = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Identification = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    IssuedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IssuedPlace = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    PowerOfAttorneyNum = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    DatePOA = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WhoPOA = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    BuisinessNumber = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    BNDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BNPlace = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Nationality = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    BankAccount = table.Column<string>(type: "varchar(50)", nullable: false),
                    BankName = table.Column<string>(type: "varchar(50)", nullable: false),
                    TaxIDNumber = table.Column<string>(type: "varchar(50)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    FAX = table.Column<string>(type: "varchar(50)", nullable: true),
                    ChargeNoticeAddress = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    BillingAddress = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    SerialPFX = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TOC_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customer_PFXCertificate_SerialPFX",
                        column: x => x.SerialPFX,
                        principalTable: "PFXCertificate",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Customer_TypeOfCustomer_TOC_ID",
                        column: x => x.TOC_ID,
                        principalTable: "TypeOfCustomer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DoneMinute",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateDone = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MinuteName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    MinuteFile = table.Column<string>(type: "nvarchar(250)", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoneMinute", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoneMinute_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OperationHistoryEmp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OperationName = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    EmployeeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationHistoryEmp", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperationHistoryEmp_Employee_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InstallationDevice",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeviceName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    DeviceStatus = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    DeviceQuantity = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    TOS_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstallationDevice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstallationDevice_TypeOfService_TOS_ID",
                        column: x => x.TOS_ID,
                        principalTable: "TypeOfService",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OperationHistoryCus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OperationName = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    CustomerID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationHistoryCus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperationHistoryCus_Customer_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PendingContract",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PContractName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    PContractFile = table.Column<string>(type: "nvarchar(250)", nullable: false),
                    IsDirector = table.Column<bool>(type: "bit", nullable: false),
                    IsCustomer = table.Column<bool>(type: "bit", nullable: false),
                    IsRefuse = table.Column<bool>(type: "bit", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InstallationAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeeCreatedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DirectorSignedId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TOS_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PendingContract", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PendingContract_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PendingContract_Employee_EmployeeCreatedId",
                        column: x => x.EmployeeCreatedId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PendingContract_TypeOfService_TOS_ID",
                        column: x => x.TOS_ID,
                        principalTable: "TypeOfService",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DoneContract",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateDone = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DConTractName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    DContractFile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsInEffect = table.Column<bool>(type: "bit", nullable: false),
                    InstallationAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeeCreatedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DirectorSignedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TOS_ID = table.Column<int>(type: "int", nullable: false),
                    DoneMinuteId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoneContract", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoneContract_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoneContract_DoneMinute_DoneMinuteId",
                        column: x => x.DoneMinuteId,
                        principalTable: "DoneMinute",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DoneContract_Employee_EmployeeCreatedId",
                        column: x => x.EmployeeCreatedId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoneContract_TypeOfService_TOS_ID",
                        column: x => x.TOS_ID,
                        principalTable: "TypeOfService",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InstallationRequirement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MinuteName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    DoneContractId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstallationRequirement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstallationRequirement_DoneContract_DoneContractId",
                        column: x => x.DoneContractId,
                        principalTable: "DoneContract",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PendingMinute",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MinuteName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    IsIntallation = table.Column<bool>(type: "bit", nullable: false),
                    IsCustomer = table.Column<bool>(type: "bit", nullable: false),
                    MinuteFile = table.Column<string>(type: "nvarchar(250)", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DoneContractId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PendingMinute", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PendingMinute_DoneContract_DoneContractId",
                        column: x => x.DoneContractId,
                        principalTable: "DoneContract",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PendingMinute_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContractCoordinate_TContractID",
                table: "ContractCoordinate",
                column: "TContractID");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_SerialPFX",
                table: "Customer",
                column: "SerialPFX");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_TOC_ID",
                table: "Customer",
                column: "TOC_ID");

            migrationBuilder.CreateIndex(
                name: "IX_DoneContract_CustomerId",
                table: "DoneContract",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_DoneContract_DoneMinuteId",
                table: "DoneContract",
                column: "DoneMinuteId");

            migrationBuilder.CreateIndex(
                name: "IX_DoneContract_EmployeeCreatedId",
                table: "DoneContract",
                column: "EmployeeCreatedId");

            migrationBuilder.CreateIndex(
                name: "IX_DoneContract_TOS_ID",
                table: "DoneContract",
                column: "TOS_ID");

            migrationBuilder.CreateIndex(
                name: "IX_DoneMinute_EmployeeId",
                table: "DoneMinute",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_PositionID",
                table: "Employee",
                column: "PositionID");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_RoleID",
                table: "Employee",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_SerialPFX",
                table: "Employee",
                column: "SerialPFX");

            migrationBuilder.CreateIndex(
                name: "IX_InstallationDevice_TOS_ID",
                table: "InstallationDevice",
                column: "TOS_ID");

            migrationBuilder.CreateIndex(
                name: "IX_InstallationRequirement_DoneContractId",
                table: "InstallationRequirement",
                column: "DoneContractId");

            migrationBuilder.CreateIndex(
                name: "IX_MinuteCoordinate_TMinuteID",
                table: "MinuteCoordinate",
                column: "TMinuteID");

            migrationBuilder.CreateIndex(
                name: "IX_OperationHistoryCus_CustomerID",
                table: "OperationHistoryCus",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_OperationHistoryEmp_EmployeeID",
                table: "OperationHistoryEmp",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_PendingContract_CustomerId",
                table: "PendingContract",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_PendingContract_EmployeeCreatedId",
                table: "PendingContract",
                column: "EmployeeCreatedId");

            migrationBuilder.CreateIndex(
                name: "IX_PendingContract_TOS_ID",
                table: "PendingContract",
                column: "TOS_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PendingMinute_DoneContractId",
                table: "PendingMinute",
                column: "DoneContractId");

            migrationBuilder.CreateIndex(
                name: "IX_PendingMinute_EmployeeId",
                table: "PendingMinute",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_TypeOfService_templateContractID",
                table: "TypeOfService",
                column: "templateContractID");

            migrationBuilder.CreateIndex(
                name: "IX_TypeOfService_templateMinuteID",
                table: "TypeOfService",
                column: "templateMinuteID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContractCoordinate");

            migrationBuilder.DropTable(
                name: "InstallationDevice");

            migrationBuilder.DropTable(
                name: "InstallationRequirement");

            migrationBuilder.DropTable(
                name: "MinuteCoordinate");

            migrationBuilder.DropTable(
                name: "OperationHistoryCus");

            migrationBuilder.DropTable(
                name: "OperationHistoryEmp");

            migrationBuilder.DropTable(
                name: "PendingContract");

            migrationBuilder.DropTable(
                name: "PendingMinute");

            migrationBuilder.DropTable(
                name: "DoneContract");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "DoneMinute");

            migrationBuilder.DropTable(
                name: "TypeOfService");

            migrationBuilder.DropTable(
                name: "TypeOfCustomer");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "TemplateContract");

            migrationBuilder.DropTable(
                name: "TemplateMinute");

            migrationBuilder.DropTable(
                name: "PFXCertificate");

            migrationBuilder.DropTable(
                name: "Position");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
