using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dierentuin.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAnimalModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Animal_Category_CategoryId",
                table: "Animal");

            migrationBuilder.DropForeignKey(
                name: "FK_Animal_Enclosure_EnclosureId",
                table: "Animal");

            migrationBuilder.DropForeignKey(
                name: "FK_Enclosure_Zoo_ZooId",
                table: "Enclosure");

            migrationBuilder.AddForeignKey(
                name: "FK_Animal_Category_CategoryId",
                table: "Animal",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Animal_Enclosure_EnclosureId",
                table: "Animal",
                column: "EnclosureId",
                principalTable: "Enclosure",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Enclosure_Zoo_ZooId",
                table: "Enclosure",
                column: "ZooId",
                principalTable: "Zoo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Animal_Category_CategoryId",
                table: "Animal");

            migrationBuilder.DropForeignKey(
                name: "FK_Animal_Enclosure_EnclosureId",
                table: "Animal");

            migrationBuilder.DropForeignKey(
                name: "FK_Enclosure_Zoo_ZooId",
                table: "Enclosure");

            migrationBuilder.AddForeignKey(
                name: "FK_Animal_Category_CategoryId",
                table: "Animal",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Animal_Enclosure_EnclosureId",
                table: "Animal",
                column: "EnclosureId",
                principalTable: "Enclosure",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Enclosure_Zoo_ZooId",
                table: "Enclosure",
                column: "ZooId",
                principalTable: "Zoo",
                principalColumn: "Id");
        }
    }
}
