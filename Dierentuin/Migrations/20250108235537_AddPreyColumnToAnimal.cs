using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dierentuin.Migrations
{
    /// <inheritdoc />
    public partial class AddPreyColumnToAnimal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Animal_Animal_AnimalId",
                table: "Animal");

            migrationBuilder.DropIndex(
                name: "IX_Animal_AnimalId",
                table: "Animal");

            migrationBuilder.DropColumn(
                name: "AnimalId",
                table: "Animal");

            migrationBuilder.AddColumn<string>(
                name: "Prey",
                table: "Animal",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Prey",
                table: "Animal");

            migrationBuilder.AddColumn<int>(
                name: "AnimalId",
                table: "Animal",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Animal_AnimalId",
                table: "Animal",
                column: "AnimalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Animal_Animal_AnimalId",
                table: "Animal",
                column: "AnimalId",
                principalTable: "Animal",
                principalColumn: "Id");
        }
    }
}
