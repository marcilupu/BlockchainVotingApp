using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlockchainVotingApp.Data.Ef.Migrations
{
    public partial class AddNoOfVotesPerElection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfVotes",
                table: "Elections",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfVotes",
                table: "Elections");
        }
    }
}
