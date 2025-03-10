using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkincareBookingSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MigrationDb_TherapistSchedule_AddPropertyReason : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "TherapistSchedules",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "277c6737-0060-4f8f-b562-052888098f84", "AQAAAAIAAYagAAAAEORPB1Hm6f7/i/30kLrWq6+30Di7grcS7kGWcra8EAu37nRzo5OaQRWg8SAivMZUjg==", "96c03138-2de5-4412-a024-31253fe488de" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Manager",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1afa8d3a-993e-4bfe-a114-68ded87611c8", "AQAAAAIAAYagAAAAEPEUuTzK0AnRmm+GQHcevO91z86oG4ZVxWtYzeimjn1iR4yZMc2kyMHVkhzB043F5Q==", "4d00b9db-30d7-4812-9f92-2db69944895b" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reason",
                table: "TherapistSchedules");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6da734df-a854-4a03-bc22-d2672e241e65", "AQAAAAIAAYagAAAAEMfRyS0AdYdneWIbPeN6R/HVMHJbkK3T2OSKpVUXCPifhquWoweCjnSgJcYpQ5G3ZA==", "f8d7d78f-9550-4a10-913c-caf2f3df798a" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Manager",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "04fbde80-f9d9-4877-b092-3077befaad64", "AQAAAAIAAYagAAAAEDX2uR/aI8zFCm27xxPZJYUv30F6W9FjltFSnWJapYZuQJq7PuKkGabuY3WS/M0cOQ==", "314659a7-704c-421d-83ac-ef5b43bf5e90" });
        }
    }
}
