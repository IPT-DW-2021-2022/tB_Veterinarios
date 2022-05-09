using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vets.Data.Migrations
{
    public partial class AdicionarSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Veterinarios",
                columns: new[] { "Id", "Fotografia", "Nome", "NumCedulaProf" },
                values: new object[] { 1, "Jose.jpg", "José Silva", "vet-8765" });

            migrationBuilder.InsertData(
                table: "Veterinarios",
                columns: new[] { "Id", "Fotografia", "Nome", "NumCedulaProf" },
                values: new object[] { 2, "Maria.jpg", "Maria Gomes dos Santos", "vet-6542" });

            migrationBuilder.InsertData(
                table: "Veterinarios",
                columns: new[] { "Id", "Fotografia", "Nome", "NumCedulaProf" },
                values: new object[] { 3, "Ricardo.jpg", "Ricardo Sousa", "vet-1339" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Veterinarios",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Veterinarios",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Veterinarios",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
