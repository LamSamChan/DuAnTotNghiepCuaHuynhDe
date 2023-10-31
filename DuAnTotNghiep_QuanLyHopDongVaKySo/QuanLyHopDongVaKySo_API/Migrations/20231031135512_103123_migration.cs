using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyHopDongVaKySo_API.Migrations
{
    /// <inheritdoc />
    public partial class _103123_migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InstallationAddress",
                table: "PendingContract",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InstallationAddress",
                table: "DoneContract",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BillingAddress",
                table: "Customer",
                type: "nvarchar(255)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ChargeNoticeAddress",
                table: "Customer",
                type: "nvarchar(255)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DatePOA",
                table: "Customer",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FAX",
                table: "Customer",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PowerOfAttorneyNum",
                table: "Customer",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WhoPOA",
                table: "Customer",
                type: "varchar(50)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InstallationAddress",
                table: "PendingContract");

            migrationBuilder.DropColumn(
                name: "InstallationAddress",
                table: "DoneContract");

            migrationBuilder.DropColumn(
                name: "BillingAddress",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "ChargeNoticeAddress",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "DatePOA",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "FAX",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "PowerOfAttorneyNum",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "WhoPOA",
                table: "Customer");
        }
    }
}
