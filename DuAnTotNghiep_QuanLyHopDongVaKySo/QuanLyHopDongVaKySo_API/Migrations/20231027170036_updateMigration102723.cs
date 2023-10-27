using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyHopDongVaKySo_API.Migrations
{
    /// <inheritdoc />
    public partial class updateMigration102723 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoneContract_Employee_EmployeeId",
                table: "DoneContract");

            migrationBuilder.DropForeignKey(
                name: "FK_PendingContract_Employee_EmployeeId",
                table: "PendingContract");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "PendingContract",
                newName: "EmployeeCreatedId");

            migrationBuilder.RenameIndex(
                name: "IX_PendingContract_EmployeeId",
                table: "PendingContract",
                newName: "IX_PendingContract_EmployeeCreatedId");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "DoneContract",
                newName: "EmployeeCreatedId");

            migrationBuilder.RenameIndex(
                name: "IX_DoneContract_EmployeeId",
                table: "DoneContract",
                newName: "IX_DoneContract_EmployeeCreatedId");

            migrationBuilder.AddColumn<Guid>(
                name: "DirectorSignedId",
                table: "PendingContract",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DirectorSignedId",
                table: "DoneContract",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_DoneContract_Employee_EmployeeCreatedId",
                table: "DoneContract",
                column: "EmployeeCreatedId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PendingContract_Employee_EmployeeCreatedId",
                table: "PendingContract",
                column: "EmployeeCreatedId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoneContract_Employee_EmployeeCreatedId",
                table: "DoneContract");

            migrationBuilder.DropForeignKey(
                name: "FK_PendingContract_Employee_EmployeeCreatedId",
                table: "PendingContract");

            migrationBuilder.DropColumn(
                name: "DirectorSignedId",
                table: "PendingContract");

            migrationBuilder.DropColumn(
                name: "DirectorSignedId",
                table: "DoneContract");

            migrationBuilder.RenameColumn(
                name: "EmployeeCreatedId",
                table: "PendingContract",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_PendingContract_EmployeeCreatedId",
                table: "PendingContract",
                newName: "IX_PendingContract_EmployeeId");

            migrationBuilder.RenameColumn(
                name: "EmployeeCreatedId",
                table: "DoneContract",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_DoneContract_EmployeeCreatedId",
                table: "DoneContract",
                newName: "IX_DoneContract_EmployeeId");

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
        }
    }
}
