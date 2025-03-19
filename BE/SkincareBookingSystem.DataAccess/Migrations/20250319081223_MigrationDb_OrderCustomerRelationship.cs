using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkincareBookingSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MigrationDb_OrderCustomerRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d7ce6a15-4f1b-436e-a7d4-a9c57d0aff29", "AQAAAAIAAYagAAAAELMkmDGKOcbL35xDeUxNvlBAwYyoUd361FLgsYFBOijXDMvb8BapdH1dBqDxoceH8w==", "57dee1f4-7661-4726-97ad-d7d0909aa693" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Manager",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1a2037e1-be9f-4d59-b376-8da422cda65b", "AQAAAAIAAYagAAAAELoF84tDBQvjY1K6ghwBB8S2PwxHQW4oVdOyCvaKdSvRit56g/D1XudiIQzwRRSJjQ==", "56cf59e0-9133-4568-b6c1-12884f0a43dc" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "db0e40be-8987-44dd-aa4c-e8e478711f90", "AQAAAAIAAYagAAAAEGoT/bo9q0FAwHmHTe4a+pIENIppadYjsDWH+XmCJ7BipQeMMT6xOpSE2v3Xd8ajYg==", "367b4002-a379-44cc-8460-c1f6d34a7321" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Manager",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4875324c-e24b-4e75-8314-d80549d7b732", "AQAAAAIAAYagAAAAENpn1jxMmbJ0bTlZQTPOIMPxxKqaykIEEQ0mRb8alEFpUbE9rv7C38WnPvF/8PdPpg==", "5c9624f4-bc85-413c-8e1f-d5f14fee87f7" });
        }
    }
}
