using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkincareBookingSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MigrationDb_AppointmentDate_DateOnly : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_ServiceType_ServiceTypeId",
                table: "Services");

            migrationBuilder.AlterColumn<Guid>(
                name: "ServiceTypeId",
                table: "Services",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "AppointmentDate",
                table: "Appointments",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Services_ServiceType_ServiceTypeId",
                table: "Services",
                column: "ServiceTypeId",
                principalTable: "ServiceType",
                principalColumn: "ServiceTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_ServiceType_ServiceTypeId",
                table: "Services");

            migrationBuilder.AlterColumn<Guid>(
                name: "ServiceTypeId",
                table: "Services",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "AppointmentDate",
                table: "Appointments",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8dc4fb3d-bf16-41f5-9a09-ccf93835814d", "AQAAAAIAAYagAAAAEOYDf0756Rg7kwD4z1OU6cetmChRMWWZxm2XENRUfNqrcVLR2ENN/kVArb9S+TYRDw==", "c2e64953-8da6-460e-b96c-65df9d2f00ef" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Manager",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7b761574-e375-4e2c-b007-4197ba3d6db8", "AQAAAAIAAYagAAAAEKSDlAzujv6XJdThJEk5ayhr0nDL2EITMlWIudxbua/wTtKt/DT1lL58GkFJpHJDTg==", "7b40e76b-5195-42fe-bca0-15d80b803455" });

            migrationBuilder.AddForeignKey(
                name: "FK_Services_ServiceType_ServiceTypeId",
                table: "Services",
                column: "ServiceTypeId",
                principalTable: "ServiceType",
                principalColumn: "ServiceTypeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
