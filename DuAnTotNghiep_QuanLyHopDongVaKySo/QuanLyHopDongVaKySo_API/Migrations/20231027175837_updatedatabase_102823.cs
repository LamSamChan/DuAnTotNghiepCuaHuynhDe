using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyHopDongVaKySo_API.Migrations
{
    /// <inheritdoc />
    public partial class updatedatabase_102823 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoneContract_DoneMinute_DoneMinuteId",
                table: "DoneContract");

            migrationBuilder.AlterColumn<int>(
                name: "DoneMinuteId",
                table: "DoneContract",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_DoneContract_DoneMinute_DoneMinuteId",
                table: "DoneContract",
                column: "DoneMinuteId",
                principalTable: "DoneMinute",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoneContract_DoneMinute_DoneMinuteId",
                table: "DoneContract");

            migrationBuilder.AlterColumn<int>(
                name: "DoneMinuteId",
                table: "DoneContract",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DoneContract_DoneMinute_DoneMinuteId",
                table: "DoneContract",
                column: "DoneMinuteId",
                principalTable: "DoneMinute",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
