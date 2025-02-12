using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkincareBookingSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Migration_DBSkinTherapist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SkinTherapist_TherapistSchedules_TherapistScheduleId",
                table: "SkinTherapist");

            migrationBuilder.AlterColumn<Guid>(
                name: "TherapistScheduleId",
                table: "SkinTherapist",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_SkinTherapist_TherapistSchedules_TherapistScheduleId",
                table: "SkinTherapist",
                column: "TherapistScheduleId",
                principalTable: "TherapistSchedules",
                principalColumn: "TherapistScheduleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SkinTherapist_TherapistSchedules_TherapistScheduleId",
                table: "SkinTherapist");

            migrationBuilder.AlterColumn<Guid>(
                name: "TherapistScheduleId",
                table: "SkinTherapist",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SkinTherapist_TherapistSchedules_TherapistScheduleId",
                table: "SkinTherapist",
                column: "TherapistScheduleId",
                principalTable: "TherapistSchedules",
                principalColumn: "TherapistScheduleId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
