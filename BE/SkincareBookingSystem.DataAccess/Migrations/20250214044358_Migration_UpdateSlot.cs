using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkincareBookingSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Migration_UpdateSlot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Slot_TherapistSchedules_TherapistScheduleId",
                table: "Slot");

            migrationBuilder.AlterColumn<Guid>(
                name: "TherapistScheduleId",
                table: "Slot",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.Sql(
                @"ALTER TABLE ""Slot"" ALTER COLUMN ""StartTime"" TYPE interval USING (""StartTime""::time - '00:00:00'::time);");

            migrationBuilder.Sql(
                @"ALTER TABLE ""Slot"" ALTER COLUMN ""EndTime"" TYPE interval USING (""EndTime""::time - '00:00:00'::time);");

            migrationBuilder.AddForeignKey(
                name: "FK_Slot_TherapistSchedules_TherapistScheduleId",
                table: "Slot",
                column: "TherapistScheduleId",
                principalTable: "TherapistSchedules",
                principalColumn: "TherapistScheduleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Slot_TherapistSchedules_TherapistScheduleId",
                table: "Slot");

            migrationBuilder.AlterColumn<Guid>(
                name: "TherapistScheduleId",
                table: "Slot",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartTime",
                table: "Slot",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "interval");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndTime",
                table: "Slot",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "interval");

            migrationBuilder.AddForeignKey(
                name: "FK_Slot_TherapistSchedules_TherapistScheduleId",
                table: "Slot",
                column: "TherapistScheduleId",
                principalTable: "TherapistSchedules",
                principalColumn: "TherapistScheduleId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
