using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TestAttemptProject.DAL.Migrations
{
    public partial class HTMLMessageEditTimeAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EditTime",
                table: "Messages",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EditDate",
                table: "HTMLMessages",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EditTime",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "EditDate",
                table: "HTMLMessages");
        }
    }
}
