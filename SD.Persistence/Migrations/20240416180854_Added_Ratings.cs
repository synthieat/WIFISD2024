using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SD.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Added_Ratings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "Rating",
                table: "Movies",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: new Guid("1e045441-dc32-4b62-9176-57e783a42ff6"),
                column: "Rating",
                value: (byte)20);

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: new Guid("39492302-d578-43a9-97fa-450312352c41"),
                column: "Rating",
                value: (byte)10);

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: new Guid("3ce3f159-c611-4d21-be1c-25311c76bf1a"),
                column: "Rating",
                value: (byte)30);

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: new Guid("746f7276-2e73-4012-b820-2651c26b3824"),
                column: "Rating",
                value: (byte)20);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Movies");
        }
    }
}
