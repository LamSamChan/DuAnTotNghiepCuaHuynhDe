using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyHopDongVaKySo_API.Migrations
{
    /// <inheritdoc />
    public partial class FourthMigration101523 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TMinuteId",
                table: "InstallationRequirement",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_InstallationRequirement_TMinuteId",
                table: "InstallationRequirement",
                column: "TMinuteId");

            migrationBuilder.AddForeignKey(
                name: "FK_InstallationRequirement_TemplateMinute_TMinuteId",
                table: "InstallationRequirement",
                column: "TMinuteId",
                principalTable: "TemplateMinute",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InstallationRequirement_TemplateMinute_TMinuteId",
                table: "InstallationRequirement");

            migrationBuilder.DropIndex(
                name: "IX_InstallationRequirement_TMinuteId",
                table: "InstallationRequirement");

            migrationBuilder.DropColumn(
                name: "TMinuteId",
                table: "InstallationRequirement");
        }
    }
}
