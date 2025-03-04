using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkincareBookingSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MigrationDb_AppointmentDateTimeNotNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AppointmentTime",
                table: "Appointments",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "AppointmentDate",
                table: "Appointments",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "13c1bcc1-b8d0-41c4-af2e-78cd54155e30", "AQAAAAIAAYagAAAAEClOe8fSAnWDKBiSTpWHEeRIvXa22qJQcDqGyCSaCpxEtUjkA4gU2KAz4vIFEpDsPg==", "09f03bd9-7668-44d2-add4-4d468379e379" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Manager",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9a6bd1c6-1852-4886-aa74-3bcda87bb4fd", "AQAAAAIAAYagAAAAEMGYwOZvTFgHxbFRsycGYacA0uWgduuYO2K2qAfhQGwhzxnLmTdg+ZnH3tuJsfEDHQ==", "61e94526-195f-4be2-8e24-81f4099d3959" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AppointmentTime",
                table: "Appointments",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<DateTime>(
                name: "AppointmentDate",
                table: "Appointments",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "296fb191-aea8-44c2-b120-1b2f79345b45", "AQAAAAIAAYagAAAAEJlezUqUljEFcuhkB5s8Ugt/gelDwSbYMUFbKQOhmv/q/HJaNdw+xEwzxLfzWxEj/Q==", "4e57e6f8-c059-48c8-816e-1f267721e19f" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Manager",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5cbe70a1-2232-4c7b-b4f6-37ccec1910df", "AQAAAAIAAYagAAAAEDqoBKGC99eun0A75QzpaTFcMv5fIM8VuhqY6ZywK38RHlZGmgVOTaXAgQQFC9j8fA==", "b16271ef-9bb6-4d00-af79-227a1be40a38" });
        }
    }
}
