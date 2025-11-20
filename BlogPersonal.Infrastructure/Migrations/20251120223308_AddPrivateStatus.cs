using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogPersonal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPrivateStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "EstadosPost",
                columns: new[] { "Id", "Nombre" },
                values: new object[] { 4, "Privado" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EstadosPost",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
