using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkincareBookingSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MigraionDb_ApplicationBirthday : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                table: "AspNetUsers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Admin",
                columns: new[] { "BirthDate", "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { new DateTime(2025, 4, 5, 8, 50, 43, 64, DateTimeKind.Utc).AddTicks(5601), "12e073f8-f161-429e-bfb5-b30fd3f48128", "AQAAAAIAAYagAAAAEGl7NTcdN95bY/A4RPmNX6ayEYNEzB1uKei8M53UbQgizteo+ZeVqBBLW7bxlhkSpQ==", "a64ea119-9db0-419d-9be5-e1115de306da" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Manager",
                columns: new[] { "BirthDate", "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { new DateTime(2025, 4, 5, 8, 50, 43, 108, DateTimeKind.Utc).AddTicks(4419), "f699304c-c8c0-43db-9781-91a57db2b41f", "AQAAAAIAAYagAAAAEHCSK9pAuakvzzn1z1De7ooJpUb4Olq6QnWmWK4grRXbgsLv0BExo45IJbCY/+QWJg==", "efdd08d0-4821-4cf3-9f15-f5c4e56d7caa" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "AspNetUsers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Admin",
                columns: new[] { "Age", "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { 30, "dc06665e-e716-439c-be67-4f0a861aada4", "AQAAAAIAAYagAAAAEDp16/VGJZ7raa8m4AM0Tjs/A1Fvpqd6JDEoErR2l5iALr2+yJv1EHXxG5hRD6rwpA==", "318e9305-a7a0-40a9-8590-d0ba66332eac" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Manager",
                columns: new[] { "Age", "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { 30, "a43f6854-48eb-49cd-ae5b-61dc87e8d557", "AQAAAAIAAYagAAAAEGqvZj4ijLOLDGZzD25bx3mQr8YbB32uU9uPNViQzZbdgWkksxESSJUmImQPhRmwHg==", "68b8759d-a33e-437a-8f75-7b28c7eff13e" });
        }
    }
}
