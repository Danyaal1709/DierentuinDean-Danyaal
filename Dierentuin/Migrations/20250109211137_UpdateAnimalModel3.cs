using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dierentuin.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAnimalModel3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Zoo_Name",
                table: "Zoo",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Enclosure_Name",
                table: "Enclosure",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Category_Name",
                table: "Category",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Zoo_Name",
                table: "Zoo");

            migrationBuilder.DropIndex(
                name: "IX_Enclosure_Name",
                table: "Enclosure");

            migrationBuilder.DropIndex(
                name: "IX_Category_Name",
                table: "Category");
        }
    }
}
