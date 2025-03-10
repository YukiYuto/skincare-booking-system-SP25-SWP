using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkincareBookingSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MigrationDb_AddTherapistScheduleStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ScheduleStatus",
                table: "TherapistSchedules",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "162e74c6-cb6c-47a4-af23-15779ea230a7", "AQAAAAIAAYagAAAAEPFiTuTla/HYtTzlGFOzt/xhM1CYFlsEfQcB08VJ0q11lI24XFSesUY/cRlRdmCZfw==", "a1966c1e-48fb-49d9-91de-f12d6239979f" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Manager",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0dab05d7-05e4-41d5-97cf-00d45bb5a873", "AQAAAAIAAYagAAAAEGbhPp98UOzV992hj2HotclLm9CupULpNSDjX8ca9JqId6rmJAV5GdyvCrrCebaDPA==", "48ace1a5-1b50-46e3-bf1c-4f8397602022" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScheduleStatus",
                table: "TherapistSchedules");

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
    }
}
