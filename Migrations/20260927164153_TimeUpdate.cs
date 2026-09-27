using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TemplumStudii.Migrations
{
    /// <inheritdoc />
    public partial class TimeUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DefinedDate",
                table: "Times",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefinedDate",
                table: "Times");
        }
    }
}
