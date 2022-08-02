using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OptoBasicNotesApi.Core.Migrations
{
    public partial class moreSeeding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Notes",
                columns: new[] { "Id", "DateCreated", "NoteBody" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "## My Seeded Note <br /> With a test script <script>alert('inject')</script>" });

            migrationBuilder.InsertData(
                table: "NoteCategories",
                columns: new[] { "Id", "CategoryId", "NoteId" },
                values: new object[] { 1, 1, 1 });

            migrationBuilder.InsertData(
                table: "NoteCategories",
                columns: new[] { "Id", "CategoryId", "NoteId" },
                values: new object[] { 2, 2, 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "NoteCategories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "NoteCategories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
