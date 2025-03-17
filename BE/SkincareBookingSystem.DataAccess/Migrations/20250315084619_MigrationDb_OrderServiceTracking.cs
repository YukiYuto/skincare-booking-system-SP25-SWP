using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkincareBookingSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MigrationDb_OrderServiceTracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ComboItemServiceComboId",
                table: "OrderDetail",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ComboItemServiceId",
                table: "OrderDetail",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OrderServiceTracking",
                columns: table => new
                {
                    TrackingId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderDetailId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderServiceTracking", x => x.TrackingId);
                    table.ForeignKey(
                        name: "FK_OrderServiceTracking_OrderDetail_OrderDetailId",
                        column: x => x.OrderDetailId,
                        principalTable: "OrderDetail",
                        principalColumn: "OrderDetailId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "df3a7ba3-91e9-46f4-b626-fe22fa715e2a", "AQAAAAIAAYagAAAAENWXuKtc7eXD5yzHmqyOAqyo71pD7CUu1FeLjmWsbIRrD2nJmTrWe127OwMVB9HfMw==", "be98cb36-7526-4e29-b055-fc4a567ccf64" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Manager",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c347f041-a582-4f59-9fd3-2176daaa63ec", "AQAAAAIAAYagAAAAEFwJKTZTbQ79OxH8efsse6pL3qhYX6bVAvgbiFnCmQ32Sp4/WAOsJsRkKSveXiZXZQ==", "917e5a36-bdfb-4f1c-b4f3-d6e422ab450f" });

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_ComboItemServiceId_ComboItemServiceComboId",
                table: "OrderDetail",
                columns: new[] { "ComboItemServiceId", "ComboItemServiceComboId" });

            migrationBuilder.CreateIndex(
                name: "IX_OrderServiceTracking_OrderDetailId",
                table: "OrderServiceTracking",
                column: "OrderDetailId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_ComboItem_ComboItemServiceId_ComboItemServiceCo~",
                table: "OrderDetail",
                columns: new[] { "ComboItemServiceId", "ComboItemServiceComboId" },
                principalTable: "ComboItem",
                principalColumns: new[] { "ServiceId", "ServiceComboId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_ComboItem_ComboItemServiceId_ComboItemServiceCo~",
                table: "OrderDetail");

            migrationBuilder.DropTable(
                name: "OrderServiceTracking");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetail_ComboItemServiceId_ComboItemServiceComboId",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "ComboItemServiceComboId",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "ComboItemServiceId",
                table: "OrderDetail");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "039e45e2-b8ff-4af9-85ee-a8e394653883", "AQAAAAIAAYagAAAAEG4CPHOeJP5aYClnehVaaAKeEJQuZpaEG/+lUsYWgIcH4kJptC6jQM3kG8EZPd4N2w==", "41db50fb-43d4-4563-84f3-bdf9ab45b120" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Manager",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d79df17b-457f-4f92-a816-5dc0147e6130", "AQAAAAIAAYagAAAAEDeVUd4xfmAZqaNdPKI//9Ttwgap1gFjK2/Inzy5NgyK2RDe6PM0rBsyQ7M9v8oM7w==", "5a4f8a1c-1f88-430a-9dc8-1a6ec810af9a" });
        }
    }
}
