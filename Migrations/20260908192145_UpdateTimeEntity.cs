using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TemplumStudii.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTimeEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value",
                table: "Times");

            migrationBuilder.DropColumn(
               name: "DefinedTime",
               table: "Times");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "DefinedTime",
                table: "Times",
                type: "interval",
                nullable: false,
                defaultValue: TimeSpan.Zero);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "AccumulatedTime",
                table: "Times",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartedAt",
                table: "Times",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccumulatedTime",
                table: "Times");

            migrationBuilder.DropColumn(
                name: "StartedAt",
                table: "Times");

            migrationBuilder.DropColumn(
                name: "DefinedTime",
                table: "Times");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "DefinedTime",
                table: "Times",
                type: "interval",
                nullable: false,
                defaultValue: TimeSpan.Zero);

            migrationBuilder.AddColumn<int>(
                name: "Value",
                table: "Times",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
