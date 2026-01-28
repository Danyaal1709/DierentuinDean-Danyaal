using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dierentuin.Migrations
{
    /// <inheritdoc />
    public partial class Zoos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Animal_Zoo_ZooId",
                table: "Animal");

            migrationBuilder.DropIndex(
                name: "IX_Animal_ZooId",
                table: "Animal");

            migrationBuilder.DropColumn(
                name: "ZooId",
                table: "Animal");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ZooId",
                table: "Animal",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Animal_ZooId",
                table: "Animal",
                column: "ZooId");

            migrationBuilder.AddForeignKey(
                name: "FK_Animal_Zoo_ZooId",
                table: "Animal",
                column: "ZooId",
                principalTable: "Zoo",
                principalColumn: "Id");
        }
    }
}
