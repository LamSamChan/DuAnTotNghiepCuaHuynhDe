using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyHopDongVaKySo_API.Migrations
{
    /// <inheritdoc />
    public partial class ThirdMigration101523 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoneContract_Employee_EmployeeId",
                table: "DoneContract");

            migrationBuilder.DropForeignKey(
                name: "FK_DoneMinute_DoneContract_Id",
                table: "DoneMinute");

            migrationBuilder.DropForeignKey(
                name: "FK_PendingContract_Employee_EmployeeId",
                table: "PendingContract");

            migrationBuilder.DropForeignKey(
                name: "FK_PendingMinute_Employee_EmployeeId",
                table: "PendingMinute");

            migrationBuilder.DropColumn(
                name: "x_CustomerZone",
                table: "TemplateMinute");

            migrationBuilder.DropColumn(
                name: "x_IntallationZone",
                table: "TemplateMinute");

            migrationBuilder.DropColumn(
                name: "y_CustomerZone",
                table: "TemplateMinute");

            migrationBuilder.DropColumn(
                name: "y_IntallationZone",
                table: "TemplateMinute");

            migrationBuilder.DropColumn(
                name: "x_CustomerZone",
                table: "TemplateContract");

            migrationBuilder.DropColumn(
                name: "x_DirectorZone",
                table: "TemplateContract");

            migrationBuilder.DropColumn(
                name: "y_CustomerZone",
                table: "TemplateContract");

            migrationBuilder.DropColumn(
                name: "y_DirectorZone",
                table: "TemplateContract");

            migrationBuilder.DropColumn(
                name: "DonContractId",
                table: "DoneMinute");

            migrationBuilder.AddColumn<string>(
                name: "jsonCustomerZone",
                table: "TemplateMinute",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "jsonIntallationZone",
                table: "TemplateMinute",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "jsonCustomerZone",
                table: "TemplateContract",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "jsonDirectorZone",
                table: "TemplateContract",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "DoneMinute",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.CreateIndex(
                name: "IX_DoneContract_DoneMinuteId",
                table: "DoneContract",
                column: "DoneMinuteId");

            migrationBuilder.AddForeignKey(
                name: "FK_DoneContract_DoneMinute_DoneMinuteId",
                table: "DoneContract",
                column: "DoneMinuteId",
                principalTable: "DoneMinute",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DoneContract_Employee_EmployeeId",
                table: "DoneContract",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PendingContract_Employee_EmployeeId",
                table: "PendingContract",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PendingMinute_Employee_EmployeeId",
                table: "PendingMinute",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoneContract_DoneMinute_DoneMinuteId",
                table: "DoneContract");

            migrationBuilder.DropForeignKey(
                name: "FK_DoneContract_Employee_EmployeeId",
                table: "DoneContract");

            migrationBuilder.DropForeignKey(
                name: "FK_PendingContract_Employee_EmployeeId",
                table: "PendingContract");

            migrationBuilder.DropForeignKey(
                name: "FK_PendingMinute_Employee_EmployeeId",
                table: "PendingMinute");

            migrationBuilder.DropIndex(
                name: "IX_DoneContract_DoneMinuteId",
                table: "DoneContract");

            migrationBuilder.DropColumn(
                name: "jsonCustomerZone",
                table: "TemplateMinute");

            migrationBuilder.DropColumn(
                name: "jsonIntallationZone",
                table: "TemplateMinute");

            migrationBuilder.DropColumn(
                name: "jsonCustomerZone",
                table: "TemplateContract");

            migrationBuilder.DropColumn(
                name: "jsonDirectorZone",
                table: "TemplateContract");

            migrationBuilder.AddColumn<int>(
                name: "x_CustomerZone",
                table: "TemplateMinute",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "x_IntallationZone",
                table: "TemplateMinute",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "y_CustomerZone",
                table: "TemplateMinute",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "y_IntallationZone",
                table: "TemplateMinute",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "x_CustomerZone",
                table: "TemplateContract",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "x_DirectorZone",
                table: "TemplateContract",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "y_CustomerZone",
                table: "TemplateContract",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "y_DirectorZone",
                table: "TemplateContract",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "DoneMinute",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<Guid>(
                name: "DonContractId",
                table: "DoneMinute",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_DoneContract_Employee_EmployeeId",
                table: "DoneContract",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DoneMinute_DoneContract_Id",
                table: "DoneMinute",
                column: "Id",
                principalTable: "DoneContract",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PendingContract_Employee_EmployeeId",
                table: "PendingContract",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PendingMinute_Employee_EmployeeId",
                table: "PendingMinute",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "Id");
        }
    }
}
