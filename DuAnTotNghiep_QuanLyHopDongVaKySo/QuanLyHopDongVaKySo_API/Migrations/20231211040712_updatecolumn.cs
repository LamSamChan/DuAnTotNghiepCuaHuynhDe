using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyHopDongVaKySo_API.Migrations
{
    /// <inheritdoc />
    public partial class updatecolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoneContract_Customer_CustomerId",
                table: "DoneContract");

            migrationBuilder.DropForeignKey(
                name: "FK_DoneContract_Employee_EmployeeCreatedId",
                table: "DoneContract");

            migrationBuilder.DropForeignKey(
                name: "FK_OperationHistoryCus_Customer_CustomerID",
                table: "OperationHistoryCus");

            migrationBuilder.DropForeignKey(
                name: "FK_OperationHistoryEmp_Employee_EmployeeID",
                table: "OperationHistoryEmp");

            migrationBuilder.AlterColumn<Guid>(
                name: "EmployeeID",
                table: "OperationHistoryEmp",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "CustomerID",
                table: "OperationHistoryCus",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "MinuteFile",
                table: "DoneMinute",
                type: "nvarchar(250)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)");

            migrationBuilder.AlterColumn<Guid>(
                name: "EmployeeCreatedId",
                table: "DoneContract",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "DirectorSignedId",
                table: "DoneContract",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "DContractFile",
                table: "DoneContract",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<Guid>(
                name: "CustomerId",
                table: "DoneContract",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateUnEffect",
                table: "DoneContract",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateAdded",
                table: "Customer",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DoneContract_Customer_CustomerId",
                table: "DoneContract",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DoneContract_Employee_EmployeeCreatedId",
                table: "DoneContract",
                column: "EmployeeCreatedId",
                principalTable: "Employee",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OperationHistoryCus_Customer_CustomerID",
                table: "OperationHistoryCus",
                column: "CustomerID",
                principalTable: "Customer",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OperationHistoryEmp_Employee_EmployeeID",
                table: "OperationHistoryEmp",
                column: "EmployeeID",
                principalTable: "Employee",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoneContract_Customer_CustomerId",
                table: "DoneContract");

            migrationBuilder.DropForeignKey(
                name: "FK_DoneContract_Employee_EmployeeCreatedId",
                table: "DoneContract");

            migrationBuilder.DropForeignKey(
                name: "FK_OperationHistoryCus_Customer_CustomerID",
                table: "OperationHistoryCus");

            migrationBuilder.DropForeignKey(
                name: "FK_OperationHistoryEmp_Employee_EmployeeID",
                table: "OperationHistoryEmp");

            migrationBuilder.DropColumn(
                name: "DateUnEffect",
                table: "DoneContract");

            migrationBuilder.DropColumn(
                name: "DateAdded",
                table: "Customer");

            migrationBuilder.AlterColumn<Guid>(
                name: "EmployeeID",
                table: "OperationHistoryEmp",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CustomerID",
                table: "OperationHistoryCus",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MinuteFile",
                table: "DoneMinute",
                type: "nvarchar(250)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "EmployeeCreatedId",
                table: "DoneContract",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "DirectorSignedId",
                table: "DoneContract",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DContractFile",
                table: "DoneContract",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CustomerId",
                table: "DoneContract",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DoneContract_Customer_CustomerId",
                table: "DoneContract",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DoneContract_Employee_EmployeeCreatedId",
                table: "DoneContract",
                column: "EmployeeCreatedId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OperationHistoryCus_Customer_CustomerID",
                table: "OperationHistoryCus",
                column: "CustomerID",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OperationHistoryEmp_Employee_EmployeeID",
                table: "OperationHistoryEmp",
                column: "EmployeeID",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
