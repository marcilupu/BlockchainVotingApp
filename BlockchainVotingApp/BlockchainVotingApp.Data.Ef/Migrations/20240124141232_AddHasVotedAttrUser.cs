using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlockchainVotingApp.Data.Ef.Migrations
{
    public partial class AddHasVotedAttrUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasVoted",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasVoted",
                table: "Users");
        }
    }
}
