using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SkincareBookingSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MigrationDb_ComboItemPrioprity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "ComboItem",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Priority",
                table: "ComboItem");

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
                    { "SkinBookingSystem-Admin", 0, "123 Admin St", 30, "5afa3eae-5dd6-40ae-94a7-923bf03e1c30", "admin@gmail.com", true, "Admin", null, "https://example.com/avatar.png", true, null, "ADMIN@GMAIL.COM", "ADMIN@GMAIL.COM", "AQAAAAIAAYagAAAAEHt1bMCLCRHK52UxrtrmMVjQVTMirNidK/ZYbhxRWVtZuoE/+cGimAj9bOYETppVKg==", "1234567890", true, "513fdfcb-232a-477b-b362-ced2519095f7", false, "admin@gmail.com" },
                    { "SkinBookingSystem-Manager", 0, "123 Manager St", 30, "af91853d-9d4e-470c-8b79-549376e8c944", "manager@gmail.com", true, "Manager", null, "https://example.com/avatarManager.png", true, null, "MANAGER@GMAIL.COM", "MANAGER@GMAIL.COM", "AQAAAAIAAYagAAAAEL4MYE9U8dF6XBPGSrfHaBQlGLI5gTNMUwediLtzaimj0mZAXDHwd3ihaQEM2W2rKQ==", "0123456789", true, "e31b5840-8c96-4907-b5c5-4dd95d117d42", false, "manager@gmail.com" }
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
    }
}
