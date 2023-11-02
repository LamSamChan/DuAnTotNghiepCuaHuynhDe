using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyHopDongVaKySo_API.Migrations
{
    /// <inheritdoc />
    public partial class _110323migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeviceQuantity",
                table: "InstallationDevice",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
               name: "MinuteFile",
               table: "InstallationRequirement",
               type: "nvarchar(250)",
               nullable: true,
               oldClrType: typeof(string),
               oldType: "nvarchar(250)",
               oldNullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeviceQuantity",
                table: "InstallationDevice");

            migrationBuilder.AlterColumn<string>(
              name: "MinuteFile",
              table: "InstallationRequirement",
              type: "nvarchar(250)",
              nullable: true,
              oldClrType: typeof(string),
              oldType: "nvarchar(250)",
              oldNullable: false);
        }
    }
}
