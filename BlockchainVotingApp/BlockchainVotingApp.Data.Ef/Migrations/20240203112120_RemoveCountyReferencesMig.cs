using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlockchainVotingApp.Data.Ef.Migrations
{
    public partial class RemoveCountyReferencesMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Elections_Counties_CountyId",
                table: "Elections");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Counties_CountyId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CountyId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Elections_CountyId",
                table: "Elections");

            migrationBuilder.DropColumn(
                name: "CountyId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CountyId",
                table: "Elections");

            migrationBuilder.AddColumn<string>(
                name: "RegisterContractAddress",
                table: "Elections",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegisterContractAddress",
                table: "Elections");

            migrationBuilder.AddColumn<int>(
                name: "CountyId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CountyId",
                table: "Elections",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_CountyId",
                table: "Users",
                column: "CountyId");

            migrationBuilder.CreateIndex(
                name: "IX_Elections_CountyId",
                table: "Elections",
                column: "CountyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Elections_Counties_CountyId",
                table: "Elections",
                column: "CountyId",
                principalTable: "Counties",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Counties_CountyId",
                table: "Users",
                column: "CountyId",
                principalTable: "Counties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
