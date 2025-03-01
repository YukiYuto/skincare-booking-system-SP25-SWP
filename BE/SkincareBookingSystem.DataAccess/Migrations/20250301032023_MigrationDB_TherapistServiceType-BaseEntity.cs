using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkincareBookingSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MigrationDB_TherapistServiceTypeBaseEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "TherapistServiceTypes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedTime",
                table: "TherapistServiceTypes",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "TherapistServiceTypes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "TherapistServiceTypes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedTime",
                table: "TherapistServiceTypes",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "TherapistServiceTypes");

            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "TherapistServiceTypes");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "TherapistServiceTypes");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "TherapistServiceTypes");

            migrationBuilder.DropColumn(
                name: "UpdatedTime",
                table: "TherapistServiceTypes");
        }
    }
}
