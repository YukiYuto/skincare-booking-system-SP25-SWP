using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkincareBookingSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MigrationDb_TherapistSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SkinTherapist_TherapistSchedules_TherapistScheduleId",
                table: "SkinTherapist");

            migrationBuilder.DropForeignKey(
                name: "FK_Slot_TherapistSchedules_TherapistScheduleId",
                table: "Slot");

            migrationBuilder.DropIndex(
                name: "IX_Slot_TherapistScheduleId",
                table: "Slot");

            migrationBuilder.DropIndex(
                name: "IX_SkinTherapist_TherapistScheduleId",
                table: "SkinTherapist");

            migrationBuilder.DropColumn(
                name: "TherapistScheduleId",
                table: "Slot");

            migrationBuilder.DropColumn(
                name: "TherapistScheduleId",
                table: "SkinTherapist");

            migrationBuilder.AddColumn<Guid>(
                name: "SlotId",
                table: "TherapistSchedules",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TherapistId",
                table: "TherapistSchedules",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "StaffCode",
                table: "Staff",
                type: "character varying(8)",
                maxLength: 8,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "IX_TherapistSchedules_AppointmentId",
                table: "TherapistSchedules",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_TherapistSchedules_SlotId",
                table: "TherapistSchedules",
                column: "SlotId");

            migrationBuilder.CreateIndex(
                name: "IX_TherapistSchedules_TherapistId",
                table: "TherapistSchedules",
                column: "TherapistId");

            migrationBuilder.AddForeignKey(
                name: "FK_TherapistSchedules_Appointments_AppointmentId",
                table: "TherapistSchedules",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "AppointmentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TherapistSchedules_SkinTherapist_TherapistId",
                table: "TherapistSchedules",
                column: "TherapistId",
                principalTable: "SkinTherapist",
                principalColumn: "SkinTherapistId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TherapistSchedules_Slot_SlotId",
                table: "TherapistSchedules",
                column: "SlotId",
                principalTable: "Slot",
                principalColumn: "SlotId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TherapistSchedules_Appointments_AppointmentId",
                table: "TherapistSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_TherapistSchedules_SkinTherapist_TherapistId",
                table: "TherapistSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_TherapistSchedules_Slot_SlotId",
                table: "TherapistSchedules");

            migrationBuilder.DropIndex(
                name: "IX_TherapistSchedules_AppointmentId",
                table: "TherapistSchedules");

            migrationBuilder.DropIndex(
                name: "IX_TherapistSchedules_SlotId",
                table: "TherapistSchedules");

            migrationBuilder.DropIndex(
                name: "IX_TherapistSchedules_TherapistId",
                table: "TherapistSchedules");

            migrationBuilder.DropColumn(
                name: "SlotId",
                table: "TherapistSchedules");

            migrationBuilder.DropColumn(
                name: "TherapistId",
                table: "TherapistSchedules");

            migrationBuilder.AlterColumn<string>(
                name: "StaffCode",
                table: "Staff",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(8)",
                oldMaxLength: 8);

            migrationBuilder.AddColumn<Guid>(
                name: "TherapistScheduleId",
                table: "Slot",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TherapistScheduleId",
                table: "SkinTherapist",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Slot_TherapistScheduleId",
                table: "Slot",
                column: "TherapistScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_SkinTherapist_TherapistScheduleId",
                table: "SkinTherapist",
                column: "TherapistScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_SkinTherapist_TherapistSchedules_TherapistScheduleId",
                table: "SkinTherapist",
                column: "TherapistScheduleId",
                principalTable: "TherapistSchedules",
                principalColumn: "TherapistScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Slot_TherapistSchedules_TherapistScheduleId",
                table: "Slot",
                column: "TherapistScheduleId",
                principalTable: "TherapistSchedules",
                principalColumn: "TherapistScheduleId");
        }
    }
}
