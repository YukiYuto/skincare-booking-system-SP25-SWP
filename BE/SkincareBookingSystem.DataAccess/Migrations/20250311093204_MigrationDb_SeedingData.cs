using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SkincareBookingSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MigrationDb_SeedingData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "8fa7c7bb-b4dd-480d-a660-e07a90855d5s", "MANAGER", "MANAGER", "MANAGER" },
                    { "8fa7c7bb-daa5-a660-bf02-82301a5eb32a", "ADMIN", "ADMIN", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "Age", "ConcurrencyStamp", "Email", "EmailConfirmed", "FullName", "Gender", "ImageUrl", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "SkinBookingSystem-Admin", 0, "123 Admin St", 30, "039e45e2-b8ff-4af9-85ee-a8e394653883", "admin@gmail.com", true, "Admin", null, "https://example.com/avatar.png", true, null, "ADMIN@GMAIL.COM", "ADMIN@GMAIL.COM", "AQAAAAIAAYagAAAAEG4CPHOeJP5aYClnehVaaAKeEJQuZpaEG/+lUsYWgIcH4kJptC6jQM3kG8EZPd4N2w==", "1234567890", true, "41db50fb-43d4-4563-84f3-bdf9ab45b120", false, "admin@gmail.com" },
                    { "SkinBookingSystem-Manager", 0, "123 Manager St", 30, "d79df17b-457f-4f92-a816-5dc0147e6130", "manager@gmail.com", true, "Manager", null, "https://example.com/avatarManager.png", true, null, "MANAGER@GMAIL.COM", "MANAGER@GMAIL.COM", "AQAAAAIAAYagAAAAEDeVUd4xfmAZqaNdPKI//9Ttwgap1gFjK2/Inzy5NgyK2RDe6PM0rBsyQ7M9v8oM7w==", "0123456789", true, "5a4f8a1c-1f88-430a-9dc8-1a6ec810af9a", false, "manager@gmail.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "8fa7c7bb-daa5-a660-bf02-82301a5eb32a", "SkinBookingSystem-Admin" },
                    { "8fa7c7bb-b4dd-480d-a660-e07a90855d5s", "SkinBookingSystem-Manager" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "8fa7c7bb-daa5-a660-bf02-82301a5eb32a", "SkinBookingSystem-Admin" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "8fa7c7bb-b4dd-480d-a660-e07a90855d5s", "SkinBookingSystem-Manager" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8fa7c7bb-b4dd-480d-a660-e07a90855d5s");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8fa7c7bb-daa5-a660-bf02-82301a5eb32a");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Admin");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Manager");
        }
    }
}
