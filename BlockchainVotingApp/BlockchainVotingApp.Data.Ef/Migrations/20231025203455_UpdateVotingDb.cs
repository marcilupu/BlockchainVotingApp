using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlockchainVotingApp.Data.Ef.Migrations
{
    public partial class UpdateVotingDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountAddress",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Voters",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Rules",
                table: "Elections",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "CountyId",
                table: "Elections",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Voters_ElectionId",
                table: "Voters",
                column: "ElectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Voters_UserId",
                table: "Voters",
                column: "UserId");

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
                name: "FK_Voters_AspNetUsers_UserId",
                table: "Voters",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Voters_Elections_ElectionId",
                table: "Voters",
                column: "ElectionId",
                principalTable: "Elections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Elections_Counties_CountyId",
                table: "Elections");

            migrationBuilder.DropForeignKey(
                name: "FK_Voters_AspNetUsers_UserId",
                table: "Voters");

            migrationBuilder.DropForeignKey(
                name: "FK_Voters_Elections_ElectionId",
                table: "Voters");

            migrationBuilder.DropIndex(
                name: "IX_Voters_ElectionId",
                table: "Voters");

            migrationBuilder.DropIndex(
                name: "IX_Voters_UserId",
                table: "Voters");

            migrationBuilder.DropIndex(
                name: "IX_Elections_CountyId",
                table: "Elections");

            migrationBuilder.DropColumn(
                name: "CountyId",
                table: "Elections");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Voters",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Rules",
                table: "Elections",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccountAddress",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
