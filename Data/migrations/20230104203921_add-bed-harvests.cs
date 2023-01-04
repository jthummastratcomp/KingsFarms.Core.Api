using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KingsFarms.Core.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class addbedharvests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Harvests",
                keyColumn: "Id",
                keyValue: 1,
                column: "HarvestDate",
                value: new DateTime(2022, 12, 25, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "Harvests",
                keyColumn: "Id",
                keyValue: 2,
                column: "HarvestDate",
                value: new DateTime(2022, 12, 30, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "Harvests",
                keyColumn: "Id",
                keyValue: 3,
                column: "HarvestDate",
                value: new DateTime(2023, 1, 4, 0, 0, 0, 0, DateTimeKind.Local));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Harvests",
                keyColumn: "Id",
                keyValue: 1,
                column: "HarvestDate",
                value: new DateTime(2022, 12, 23, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "Harvests",
                keyColumn: "Id",
                keyValue: 2,
                column: "HarvestDate",
                value: new DateTime(2022, 12, 28, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "Harvests",
                keyColumn: "Id",
                keyValue: 3,
                column: "HarvestDate",
                value: new DateTime(2023, 1, 2, 0, 0, 0, 0, DateTimeKind.Local));
        }
    }
}
