using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkincareBookingSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MigrationDb_AppointmentDurationNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "DurationMinutes",
                table: "Appointments",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "542cc928-825b-427f-8296-d3ae49ebdaa9", "AQAAAAIAAYagAAAAEN4KS3jg7hsDzZJOGUyknwiYd2AQpW6YubXNvFk8NvW3RESNT1XlbKIKwULadtYvyg==", "eeb19498-6a15-42fc-99e5-362d409d2934" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Manager",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "db1f7b38-bf2a-4ee1-952f-bf38e3090bba", "AQAAAAIAAYagAAAAEPJdqe7Upqa9B1j3sQNW6bgnNJuxlpUhnMuv0lBrs8JVDIFWLnOdrMi63TLeHnTlMw==", "db1293c6-dabd-4655-b4b1-d2e1555e880e" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "DurationMinutes",
                table: "Appointments",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

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
    }
}
