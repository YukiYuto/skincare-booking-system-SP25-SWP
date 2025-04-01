using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkincareBookingSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MigrationDb_AppointmentDuration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DurationMinutes",
                table: "Appointments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "301d1243-05ee-4a86-a791-fb3271ff3e1c", "AQAAAAIAAYagAAAAECnQUXO/sJ15MF/FBXDKrqxM+h0FhoLULbAQ3eQ95yzb1QKviBEv3X9zyFFWwMv99Q==", "a24b6851-e482-4ab3-95d3-cfa61ad6a743" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Manager",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "bbbe75e7-ad7c-4ce5-b94c-63212273e02d", "AQAAAAIAAYagAAAAEMyBNhGkSYZgVnzGjZYj4xREhvwU4HlcC9txpbeu1/OF+CjmYRG3VNVn/9mhWg7VFA==", "18ba4fcc-f7c2-425d-84e8-b6c84764731b" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DurationMinutes",
                table: "Appointments");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "df3a7ba3-91e9-46f4-b626-fe22fa715e2a", "AQAAAAIAAYagAAAAENWXuKtc7eXD5yzHmqyOAqyo71pD7CUu1FeLjmWsbIRrD2nJmTrWe127OwMVB9HfMw==", "be98cb36-7526-4e29-b055-fc4a567ccf64" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Manager",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c347f041-a582-4f59-9fd3-2176daaa63ec", "AQAAAAIAAYagAAAAEFwJKTZTbQ79OxH8efsse6pL3qhYX6bVAvgbiFnCmQ32Sp4/WAOsJsRkKSveXiZXZQ==", "917e5a36-bdfb-4f1c-b4f3-d6e422ab450f" });
        }
    }
}
