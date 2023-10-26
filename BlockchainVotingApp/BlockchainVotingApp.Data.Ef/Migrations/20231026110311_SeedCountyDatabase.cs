using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlockchainVotingApp.Data.Ef.Migrations
{
    public partial class SeedCountyDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Alba" },
                    { 2, "Arad" },
                    { 3, "Arges" },
                    { 4, "Bihor" },
                    { 5, "Brasov" },
                    { 6, "Braila" },
                    { 7, "Bucuresti" },
                    { 8, "Cluj" },
                    { 9, "Constanta" },
                    { 10, "Covasna" },
                    { 11, "Dolj" },
                    { 12, "Galati" },
                    { 13, "Gorj" },
                    { 14, "Giurgiu" },
                    { 15, "Hunedoara" },
                    { 16, "Ialomita" },
                    { 17, "Iasi" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Counties",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Counties",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Counties",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Counties",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Counties",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Counties",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Counties",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Counties",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Counties",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Counties",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Counties",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Counties",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Counties",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Counties",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Counties",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Counties",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Counties",
                keyColumn: "Id",
                keyValue: 17);
        }
    }
}
