using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KingsFarms.Core.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class BedAndHarvestSeed1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Beds",
                columns: new[] { "Id", "Name", "Section" },
                values: new object[,]
                {
                    { 1, "1", "MidWest" },
                    { 2, "2", "MidWest" },
                    { 3, "3", "MidWest" }
                });

            migrationBuilder.InsertData(
                table: "Harvests",
                columns: new[] { "Id", "BedId", "HarvestDate", "Quantity" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2022, 12, 23, 0, 0, 0, 0, DateTimeKind.Local), 230 },
                    { 2, null, new DateTime(2022, 12, 28, 0, 0, 0, 0, DateTimeKind.Local), 120 },
                    { 3, null, new DateTime(2023, 1, 2, 0, 0, 0, 0, DateTimeKind.Local), 24 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Beds",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Beds",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Beds",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Harvests",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Harvests",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Harvests",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
