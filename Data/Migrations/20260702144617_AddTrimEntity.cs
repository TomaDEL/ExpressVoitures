using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressVoitures.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTrimEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Trim",
                table: "Cars");

            migrationBuilder.AddColumn<int>(
                name: "TrimId",
                table: "Cars",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Trims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CarModelId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trims_CarModels_CarModelId",
                        column: x => x.CarModelId,
                        principalTable: "CarModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cars_TrimId",
                table: "Cars",
                column: "TrimId");

            migrationBuilder.CreateIndex(
                name: "IX_Trims_CarModelId",
                table: "Trims",
                column: "CarModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_Trims_TrimId",
                table: "Cars",
                column: "TrimId",
                principalTable: "Trims",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_Trims_TrimId",
                table: "Cars");

            migrationBuilder.DropTable(
                name: "Trims");

            migrationBuilder.DropIndex(
                name: "IX_Cars_TrimId",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "TrimId",
                table: "Cars");

            migrationBuilder.AddColumn<string>(
                name: "Trim",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
