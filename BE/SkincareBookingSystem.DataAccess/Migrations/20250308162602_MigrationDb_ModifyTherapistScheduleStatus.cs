using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkincareBookingSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MigrationDb_ModifyTherapistScheduleStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ScheduleStatus",
                table: "TherapistSchedules",
                type: "varchar(20)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0b623595-033f-436c-b3fc-dad4602f0385", "AQAAAAIAAYagAAAAEBiXWvB0uBe2YdAF3U5Rj1AAYpMYudbhMfkT0KrNiDw3riku+ifLlipTuQ4WrxSWWg==", "814f0a3f-f269-4e73-9400-cada5de952d3" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Manager",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6bac633f-ebe8-4909-bda1-258e4f939409", "AQAAAAIAAYagAAAAEPdC/eYgb3vwQWdiEX+W3Oql80WJ3UKHNRfapTiJtxFlKN3wnnY2YPM/aMjyrRxzkw==", "ec02e817-3faf-420c-8566-fb48f0f2a55c" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ScheduleStatus",
                table: "TherapistSchedules",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(20)");

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
    }
}
