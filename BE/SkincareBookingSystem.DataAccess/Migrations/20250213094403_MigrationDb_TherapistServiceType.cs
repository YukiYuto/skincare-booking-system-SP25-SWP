using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkincareBookingSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MigrationDb_TherapistServiceType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_ServiceCombo_ServiceComboId",
                table: "OrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Services_ServiceId",
                table: "OrderDetail");

            migrationBuilder.AddColumn<Guid>(
                name: "SkinTherapistId",
                table: "SkinServiceType",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ServiceId",
                table: "OrderDetail",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "ServiceComboId",
                table: "OrderDetail",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateTable(
                name: "TherapistServiceTypes",
                columns: table => new
                {
                    TherapistId = table.Column<Guid>(type: "uuid", nullable: false),
                    ServiceTypeId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TherapistServiceTypes", x => new { x.TherapistId, x.ServiceTypeId });
                    table.ForeignKey(
                        name: "FK_TherapistServiceTypes_ServiceType_ServiceTypeId",
                        column: x => x.ServiceTypeId,
                        principalTable: "ServiceType",
                        principalColumn: "ServiceTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TherapistServiceTypes_SkinTherapist_TherapistId",
                        column: x => x.TherapistId,
                        principalTable: "SkinTherapist",
                        principalColumn: "SkinTherapistId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SkinServiceType_SkinTherapistId",
                table: "SkinServiceType",
                column: "SkinTherapistId");

            migrationBuilder.CreateIndex(
                name: "IX_TherapistServiceTypes_ServiceTypeId",
                table: "TherapistServiceTypes",
                column: "ServiceTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_ServiceCombo_ServiceComboId",
                table: "OrderDetail",
                column: "ServiceComboId",
                principalTable: "ServiceCombo",
                principalColumn: "ServiceComboId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Services_ServiceId",
                table: "OrderDetail",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_SkinServiceType_SkinTherapist_SkinTherapistId",
                table: "SkinServiceType",
                column: "SkinTherapistId",
                principalTable: "SkinTherapist",
                principalColumn: "SkinTherapistId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_ServiceCombo_ServiceComboId",
                table: "OrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Services_ServiceId",
                table: "OrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_SkinServiceType_SkinTherapist_SkinTherapistId",
                table: "SkinServiceType");

            migrationBuilder.DropTable(
                name: "TherapistServiceTypes");

            migrationBuilder.DropIndex(
                name: "IX_SkinServiceType_SkinTherapistId",
                table: "SkinServiceType");

            migrationBuilder.DropColumn(
                name: "SkinTherapistId",
                table: "SkinServiceType");

            migrationBuilder.AlterColumn<Guid>(
                name: "ServiceId",
                table: "OrderDetail",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ServiceComboId",
                table: "OrderDetail",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_ServiceCombo_ServiceComboId",
                table: "OrderDetail",
                column: "ServiceComboId",
                principalTable: "ServiceCombo",
                principalColumn: "ServiceComboId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Services_ServiceId",
                table: "OrderDetail",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "ServiceId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
