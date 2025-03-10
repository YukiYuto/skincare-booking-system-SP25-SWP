using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkincareBookingSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MigrationDb_RemoveServiceTypeId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_ServiceType_ServiceTypeId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_ServiceTypeId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "ServiceTypeId",
                table: "Services");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5afa3eae-5dd6-40ae-94a7-923bf03e1c30", "AQAAAAIAAYagAAAAEHt1bMCLCRHK52UxrtrmMVjQVTMirNidK/ZYbhxRWVtZuoE/+cGimAj9bOYETppVKg==", "513fdfcb-232a-477b-b362-ced2519095f7" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Manager",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "af91853d-9d4e-470c-8b79-549376e8c944", "AQAAAAIAAYagAAAAEL4MYE9U8dF6XBPGSrfHaBQlGLI5gTNMUwediLtzaimj0mZAXDHwd3ihaQEM2W2rKQ==", "e31b5840-8c96-4907-b5c5-4dd95d117d42" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ServiceTypeId",
                table: "Services",
                type: "uuid",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_Services_ServiceTypeId",
                table: "Services",
                column: "ServiceTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_ServiceType_ServiceTypeId",
                table: "Services",
                column: "ServiceTypeId",
                principalTable: "ServiceType",
                principalColumn: "ServiceTypeId");
        }
    }
}
