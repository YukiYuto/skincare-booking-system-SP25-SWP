using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkincareBookingSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MigrationDb_CheckInOutTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CheckInTime",
                table: "Appointments",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckOutTime",
                table: "Appointments",
                type: "timestamp with time zone",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CheckInTime",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "CheckOutTime",
                table: "Appointments");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ddbd404e-a7d2-428f-9c55-53bc2a659542", "AQAAAAIAAYagAAAAED3HGI3sD8TgAzTXpcsSpuBgn2Zfoxg0gWeyxcylimAHB2YGrHPlT58LZq5kxJNN+Q==", "10424970-4ab9-4765-b910-91bcf23df9fe" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Manager",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fdfc587c-be31-44b1-9225-2fd1fc94674d", "AQAAAAIAAYagAAAAEDHAMFoTtkzGEb8qd5tJAGrvaa8hfJz1WISPh2OQ76Z58MLr1F7Qh81E1ESnH9Qp7Q==", "255b04c0-7b04-428e-b539-54b20e6e2a3e" });
        }
    }
}
