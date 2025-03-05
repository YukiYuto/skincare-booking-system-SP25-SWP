using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkincareBookingSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MigrationDb_ServiceTypeId : Migration
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

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Services",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

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

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Services",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d30f8a7f-54b3-400c-8a02-855980bdf9a8", "AQAAAAIAAYagAAAAEEMuHgekTnud0xVO9W7bTlOW86DFlWXc3wUxbQgstqQrwTBl6rNTR0BJEpL3a+J9EQ==", "e18acb7a-c3fa-49e3-95fa-961032622bd0" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Manager",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8ba8497e-d5a0-47a2-aac5-43e9ea7d0087", "AQAAAAIAAYagAAAAEDJE40neYI2HpIsNtxHLydXkA77STQ8A2+r19BOS11H/HgCXq1Dft0xabW4t8Lbkvw==", "f5bf76e3-395c-49ed-b44f-5751dee005cf" });

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
