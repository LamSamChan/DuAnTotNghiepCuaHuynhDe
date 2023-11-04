using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyHopDongVaKySo_API.Migrations
{
    /// <inheritdoc />
    public partial class updateMigration_051123 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_TypeOfCustomer_TOC_ID",
                table: "Customer");

            migrationBuilder.DropTable(
                name: "TypeOfCustomer");

            migrationBuilder.DropIndex(
                name: "IX_Customer_TOC_ID",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "TOC_ID",
                table: "Customer");

            migrationBuilder.AddColumn<string>(
                name: "typeofCustomer",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "typeofCustomer",
                table: "Customer");

            migrationBuilder.AddColumn<int>(
                name: "TOC_ID",
                table: "Customer",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.CreateIndex(
                name: "IX_Customer_TOC_ID",
                table: "Customer",
                column: "TOC_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_TypeOfCustomer_TOC_ID",
                table: "Customer",
                column: "TOC_ID",
                principalTable: "TypeOfCustomer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
