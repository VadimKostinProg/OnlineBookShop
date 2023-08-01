using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookShop.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedDataCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "DisplayOrder", "Name" },
                values: new object[,]
                {
                    { new Guid("5778ab89-fb71-4277-9c77-866ad3f89bb7"), 2, "Comedia" },
                    { new Guid("a0435e87-94eb-4ae2-8753-4a6d984efa88"), 3, "History" },
                    { new Guid("f89d4b2e-bdfd-464d-8f8b-230a39ec5e9b"), 1, "Action" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("5778ab89-fb71-4277-9c77-866ad3f89bb7"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("a0435e87-94eb-4ae2-8753-4a6d984efa88"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("f89d4b2e-bdfd-464d-8f8b-230a39ec5e9b"));
        }
    }
}
