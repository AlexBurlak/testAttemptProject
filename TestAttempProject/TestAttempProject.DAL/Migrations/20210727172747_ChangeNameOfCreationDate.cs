using Microsoft.EntityFrameworkCore.Migrations;

namespace TestAttemptProject.DAL.Migrations
{
    public partial class ChangeNameOfCreationDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DataStamp",
                table: "Messages",
                newName: "CreationStamp");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreationStamp",
                table: "Messages",
                newName: "DataStamp");
        }
    }
}
