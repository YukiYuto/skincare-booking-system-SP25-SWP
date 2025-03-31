using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkincareBookingSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MigrationDb_NullableCustomerSkinTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SkinTest_CustomerSkinTest_CustomerSkinTestId",
                table: "SkinTest");

            migrationBuilder.AlterColumn<Guid>(
                name: "CustomerSkinTestId",
                table: "SkinTest",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6d521d75-0c58-4c1c-939d-d1448d731ea8", "AQAAAAIAAYagAAAAEO9dq9oXAWwnG+VNmxmpOPJ1Y5RPp+YXxLRhcVwK5vEMgeh1nP4koDvPkYx02pacYQ==", "17897c4f-09e7-4e92-9b49-850c0962249e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Manager",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c09d7ced-1a40-4807-b3ef-844fad0d71bc", "AQAAAAIAAYagAAAAEPdozSa5EKXba9VH/v1m/sqST5WjEqrOn+wQysS0DqiPS7x+6KJTI9Ps1FXVNEAF8g==", "52b93042-fd07-4f0c-89ab-01ee5e59094d" });

            migrationBuilder.AddForeignKey(
                name: "FK_SkinTest_CustomerSkinTest_CustomerSkinTestId",
                table: "SkinTest",
                column: "CustomerSkinTestId",
                principalTable: "CustomerSkinTest",
                principalColumn: "CustomerSkinTestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SkinTest_CustomerSkinTest_CustomerSkinTestId",
                table: "SkinTest");

            migrationBuilder.AlterColumn<Guid>(
                name: "CustomerSkinTestId",
                table: "SkinTest",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_SkinTest_CustomerSkinTest_CustomerSkinTestId",
                table: "SkinTest",
                column: "CustomerSkinTestId",
                principalTable: "CustomerSkinTest",
                principalColumn: "CustomerSkinTestId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
