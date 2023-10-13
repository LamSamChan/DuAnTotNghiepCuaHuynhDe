using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyHopDongVaKySo_API.Migrations
{
    /// <inheritdoc />
    public partial class SecondMigration_101323 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InstallationMinute_DoneContract_DoneContractId",
                table: "InstallationMinute");

            migrationBuilder.DropForeignKey(
                name: "FK_InstallationMinute_Employee_EmployeeId",
                table: "InstallationMinute");

            migrationBuilder.DropForeignKey(
                name: "FK_InstallationMinute_TemplateMinute_TMinuteId",
                table: "InstallationMinute");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InstallationMinute",
                table: "InstallationMinute");

            migrationBuilder.RenameTable(
                name: "InstallationMinute",
                newName: "PendingMinute");

            migrationBuilder.RenameIndex(
                name: "IX_InstallationMinute_TMinuteId",
                table: "PendingMinute",
                newName: "IX_PendingMinute_TMinuteId");

            migrationBuilder.RenameIndex(
                name: "IX_InstallationMinute_EmployeeId",
                table: "PendingMinute",
                newName: "IX_PendingMinute_EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_InstallationMinute_DoneContractId",
                table: "PendingMinute",
                newName: "IX_PendingMinute_DoneContractId");

            migrationBuilder.AddColumn<bool>(
                name: "isHidden",
                table: "Role",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isHidden",
                table: "Position",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PendingMinute",
                table: "PendingMinute",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PendingMinute_DoneContract_DoneContractId",
                table: "PendingMinute",
                column: "DoneContractId",
                principalTable: "DoneContract",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PendingMinute_Employee_EmployeeId",
                table: "PendingMinute",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PendingMinute_TemplateMinute_TMinuteId",
                table: "PendingMinute",
                column: "TMinuteId",
                principalTable: "TemplateMinute",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PendingMinute_DoneContract_DoneContractId",
                table: "PendingMinute");

            migrationBuilder.DropForeignKey(
                name: "FK_PendingMinute_Employee_EmployeeId",
                table: "PendingMinute");

            migrationBuilder.DropForeignKey(
                name: "FK_PendingMinute_TemplateMinute_TMinuteId",
                table: "PendingMinute");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PendingMinute",
                table: "PendingMinute");

            migrationBuilder.DropColumn(
                name: "isHidden",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "isHidden",
                table: "Position");

            migrationBuilder.RenameTable(
                name: "PendingMinute",
                newName: "InstallationMinute");

            migrationBuilder.RenameIndex(
                name: "IX_PendingMinute_TMinuteId",
                table: "InstallationMinute",
                newName: "IX_InstallationMinute_TMinuteId");

            migrationBuilder.RenameIndex(
                name: "IX_PendingMinute_EmployeeId",
                table: "InstallationMinute",
                newName: "IX_InstallationMinute_EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_PendingMinute_DoneContractId",
                table: "InstallationMinute",
                newName: "IX_InstallationMinute_DoneContractId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InstallationMinute",
                table: "InstallationMinute",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InstallationMinute_DoneContract_DoneContractId",
                table: "InstallationMinute",
                column: "DoneContractId",
                principalTable: "DoneContract",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InstallationMinute_Employee_EmployeeId",
                table: "InstallationMinute",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InstallationMinute_TemplateMinute_TMinuteId",
                table: "InstallationMinute",
                column: "TMinuteId",
                principalTable: "TemplateMinute",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
