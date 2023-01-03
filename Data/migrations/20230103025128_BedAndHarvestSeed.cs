using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KingsFarms.Core.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class BedAndHarvestSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BedId",
                table: "Harvests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "HarvestDate",
                table: "Harvests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Harvests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Beds",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Section",
                table: "Beds",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Harvests_BedId",
                table: "Harvests",
                column: "BedId");

            migrationBuilder.AddForeignKey(
                name: "FK_Harvests_Beds_BedId",
                table: "Harvests",
                column: "BedId",
                principalTable: "Beds",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Harvests_Beds_BedId",
                table: "Harvests");

            migrationBuilder.DropIndex(
                name: "IX_Harvests_BedId",
                table: "Harvests");

            migrationBuilder.DropColumn(
                name: "BedId",
                table: "Harvests");

            migrationBuilder.DropColumn(
                name: "HarvestDate",
                table: "Harvests");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Harvests");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Beds");

            migrationBuilder.DropColumn(
                name: "Section",
                table: "Beds");
        }
    }
}
