using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkincareBookingSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MigrationDb_BaseEntityTestQA : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_ServiceType_ServiceTypeId",
                table: "Services");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "TestQuestion",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedTime",
                table: "TestQuestion",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "TestQuestion",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "TestQuestion",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedTime",
                table: "TestQuestion",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "TestAnswer",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedTime",
                table: "TestAnswer",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "TestAnswer",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "TestAnswer",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedTime",
                table: "TestAnswer",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ServiceTypeId",
                table: "Services",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "63303d9a-4c90-41d1-968f-d6858971a2f1", "AQAAAAIAAYagAAAAEHpoKy4wQ7hBAwgCSwCzjAcmaVqZTr2FaODlX8Ofg+weJPHTq1fqzioY4+fT+tbFIw==", "2482a74f-7de0-4cfa-820f-003807fc9d74" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Manager",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e3e47dbe-2f66-4414-bb4e-0009a5456283", "AQAAAAIAAYagAAAAEFY6t0zwO3974nPrP4LHoAqJ7h7fgcSi0BSmxYsImjKwQVA5lWqA7Omp8vF/W1gn1w==", "66adf8e7-50c7-458f-b132-4e80825ef1e2" });

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

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "TestQuestion");

            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "TestQuestion");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "TestQuestion");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "TestQuestion");

            migrationBuilder.DropColumn(
                name: "UpdatedTime",
                table: "TestQuestion");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "TestAnswer");

            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "TestAnswer");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "TestAnswer");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "TestAnswer");

            migrationBuilder.DropColumn(
                name: "UpdatedTime",
                table: "TestAnswer");

            migrationBuilder.AlterColumn<Guid>(
                name: "ServiceTypeId",
                table: "Services",
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
