using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ApiOrnek.Migrations
{
    /// <inheritdoc />
    public partial class BaslikTablosu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Metin",
                table: "notlar",
                newName: "Text");

            migrationBuilder.AddColumn<int>(
                name: "BaslikId",
                table: "notlar",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Basliklar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Basliklar", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_notlar_BaslikId",
                table: "notlar",
                column: "BaslikId");

            migrationBuilder.AddForeignKey(
                name: "FK_notlar_Basliklar_BaslikId",
                table: "notlar",
                column: "BaslikId",
                principalTable: "Basliklar",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_notlar_Basliklar_BaslikId",
                table: "notlar");

            migrationBuilder.DropTable(
                name: "Basliklar");

            migrationBuilder.DropIndex(
                name: "IX_notlar_BaslikId",
                table: "notlar");

            migrationBuilder.DropColumn(
                name: "BaslikId",
                table: "notlar");

            migrationBuilder.RenameColumn(
                name: "Text",
                table: "notlar",
                newName: "Metin");
        }
    }
}
