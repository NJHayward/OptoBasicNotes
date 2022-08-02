using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OptoBasicNotesApi.Core.Migrations
{
    public partial class moreSeedingcorrecttext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: 1,
                column: "NoteBody",
                value: "## My Seeded Note \n With a test script <script>alert('inject')</script>");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: 1,
                column: "NoteBody",
                value: "## My Seeded Note <br /> With a test script <script>alert('inject')</script>");
        }
    }
}
