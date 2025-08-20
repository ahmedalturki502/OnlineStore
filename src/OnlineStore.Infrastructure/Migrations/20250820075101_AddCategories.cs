using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace E_Commerce.Migrations
{
    /// <inheritdoc />
    public partial class AddCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("a5e4a8f9-5e7b-4f40-9742-880abb3bf023"), null, "Customer", "CUSTOMER" },
                    { new Guid("aad84b5a-7585-4446-b334-5432a33be272"), null, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UpdatedAt", "UserName" },
                values: new object[] { new Guid("47162438-f2ca-4817-bb56-8dbb8ab1c67b"), 0, "7cf929e2-ee30-47c4-bd2e-24e436e9f83a", new DateTime(2025, 8, 20, 7, 51, 0, 177, DateTimeKind.Utc).AddTicks(5025), "admin@onlinestore.com", true, "Store Admin", false, null, "ADMIN@ONLINESTORE.COM", "ADMIN@ONLINESTORE.COM", "AQAAAAIAAYagAAAAENv88kmZg1BsKheSzk93i+i7ywsOTFC9UBO0pKaNNNymv4cC1i0xCWg7DqrEv8JhBg==", null, false, "7af35a60-d625-41e7-8620-ff0fe650e0aa", false, new DateTime(2025, 8, 20, 7, 51, 0, 177, DateTimeKind.Utc).AddTicks(5028), "admin@onlinestore.com" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("6240efe9-7ac5-42f5-a7cf-4b1d4f1b3f4a"), new DateTime(2025, 8, 20, 7, 51, 0, 366, DateTimeKind.Utc).AddTicks(9773), "Electronic devices, gadgets, and accessories", "Electronics", new DateTime(2025, 8, 20, 7, 51, 0, 366, DateTimeKind.Utc).AddTicks(9775) },
                    { new Guid("662e2134-28a0-45ab-934b-41160de6d926"), new DateTime(2025, 8, 20, 7, 51, 0, 366, DateTimeKind.Utc).AddTicks(9809), "Apparel, shoes, and fashion accessories", "Clothing", new DateTime(2025, 8, 20, 7, 51, 0, 366, DateTimeKind.Utc).AddTicks(9811) },
                    { new Guid("94f5fe5e-af92-4496-bb97-9b7984a66200"), new DateTime(2025, 8, 20, 7, 51, 0, 366, DateTimeKind.Utc).AddTicks(9788), "Food items, beverages, and household essentials", "Groceries", new DateTime(2025, 8, 20, 7, 51, 0, 366, DateTimeKind.Utc).AddTicks(9790) }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("aad84b5a-7585-4446-b334-5432a33be272"), new Guid("47162438-f2ca-4817-bb56-8dbb8ab1c67b") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("a5e4a8f9-5e7b-4f40-9742-880abb3bf023"));

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("aad84b5a-7585-4446-b334-5432a33be272"), new Guid("47162438-f2ca-4817-bb56-8dbb8ab1c67b") });

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("6240efe9-7ac5-42f5-a7cf-4b1d4f1b3f4a"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("662e2134-28a0-45ab-934b-41160de6d926"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("94f5fe5e-af92-4496-bb97-9b7984a66200"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("aad84b5a-7585-4446-b334-5432a33be272"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("47162438-f2ca-4817-bb56-8dbb8ab1c67b"));
        }
    }
}
