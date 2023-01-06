using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KingsFarms.Core.Api.data.migrations
{
    /// <inheritdoc />
    public partial class BedNameNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Beds_Name",
                table: "Beds");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Beds");

            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "Beds",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Beds_Number",
                table: "Beds",
                column: "Number",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Beds_Number",
                table: "Beds");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "Beds");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Beds",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Beds_Name",
                table: "Beds",
                column: "Name",
                unique: true);
        }
    }
}
