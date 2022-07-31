using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OptoBasicNotesApi.Core.Migrations
{
    public partial class fieldsAndRelationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Notes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "NoteBody",
                table: "Notes",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CategoryName",
                table: "Categories",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "NoteCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoteId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NoteCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NoteCategories_Notes_NoteId",
                        column: x => x.NoteId,
                        principalTable: "Notes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NoteCategories_CategoryId",
                table: "NoteCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_NoteCategories_NoteId",
                table: "NoteCategories",
                column: "NoteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NoteCategories");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "NoteBody",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "CategoryName",
                table: "Categories");
        }
    }
}
