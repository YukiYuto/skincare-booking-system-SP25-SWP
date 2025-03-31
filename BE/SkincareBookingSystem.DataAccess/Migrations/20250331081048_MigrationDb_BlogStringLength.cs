using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkincareBookingSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MigrationDb_BlogStringLength : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Blogs",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Blogs",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2a8e0972-9563-478a-91c8-d41e26e1b426", "AQAAAAIAAYagAAAAEFhgAdT9xiPCyqCbKgPtbOhhAdSyp10Ow2/wUjw7HsOv+EybO5MWJohf8wT2ETEggw==", "c9a93af9-8634-4dc3-9e89-1b8c73a315e7" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Manager",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e74c2a40-be01-46c5-85da-43a2096ba52d", "AQAAAAIAAYagAAAAENca9oN87vwIYYp9XFw8RKFw9/KXhVW36+cGAPGD4Cp/pEWm/BTGkEHKmoEj/ACnCg==", "d2523188-5735-4fc3-bec7-b10890c7fe19" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Blogs",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Blogs",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e026a5c5-5fe8-4983-93f0-2d1845a62cf0", "AQAAAAIAAYagAAAAEI7BjcxW9wAEav6LG9AMiOwwD6q7HlPyNtCPdPSe3CCQsrHk4WTqKOgcp7wFnDyQag==", "f870ab78-360a-4e13-a547-b64d5d32dccb" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Manager",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5b9a3588-d3f3-47be-85b3-55a71d961c27", "AQAAAAIAAYagAAAAEIPcmXnu60sRhHOHItNpXts7A7unCSNgpIRy6CxxE3SC0yYL3ODvez7g+4RS+3ftGg==", "2bd08bc2-7987-4435-a60f-6ef19a1bb97c" });
        }
    }
}
