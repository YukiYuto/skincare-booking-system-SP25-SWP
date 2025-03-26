using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SkincareBookingSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MigrationDb_Seeding : Migration
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
                    { "SkinBookingSystem-Admin", 0, "123 Admin St", 30, "296fb191-aea8-44c2-b120-1b2f79345b45", "admin@gmail.com", true, "Admin", null, "https://example.com/avatar.png", true, null, "ADMIN@GMAIL.COM", "ADMIN@GMAIL.COM", "AQAAAAIAAYagAAAAEJlezUqUljEFcuhkB5s8Ugt/gelDwSbYMUFbKQOhmv/q/HJaNdw+xEwzxLfzWxEj/Q==", "1234567890", true, "4e57e6f8-c059-48c8-816e-1f267721e19f", false, "admin@gmail.com" },
                    { "SkinBookingSystem-Manager", 0, "123 Manager St", 30, "5cbe70a1-2232-4c7b-b4f6-37ccec1910df", "manager@gmail.com", true, "Manager", null, "https://example.com/avatarManager.png", true, null, "MANAGER@GMAIL.COM", "MANAGER@GMAIL.COM", "AQAAAAIAAYagAAAAEDqoBKGC99eun0A75QzpaTFcMv5fIM8VuhqY6ZywK38RHlZGmgVOTaXAgQQFC9j8fA==", "0123456789", true, "b16271ef-9bb6-4d00-af79-227a1be40a38", false, "manager@gmail.com" }
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
