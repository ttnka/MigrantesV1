using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TorneosV2.Migrations
{
    /// <inheritdoc />
    public partial class Inicial3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrgId",
                table: "Servicios",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Servicios_OrgId",
                table: "Servicios",
                column: "OrgId");

            migrationBuilder.AddForeignKey(
                name: "FK_Servicios_Organizaciones_OrgId",
                table: "Servicios",
                column: "OrgId",
                principalTable: "Organizaciones",
                principalColumn: "OrgId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Servicios_Organizaciones_OrgId",
                table: "Servicios");

            migrationBuilder.DropIndex(
                name: "IX_Servicios_OrgId",
                table: "Servicios");

            migrationBuilder.DropColumn(
                name: "OrgId",
                table: "Servicios");
        }
    }
}
