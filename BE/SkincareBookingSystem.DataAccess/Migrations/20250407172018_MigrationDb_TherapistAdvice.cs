using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkincareBookingSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MigrationDb_TherapistAdvice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TherapistAdvice",
                columns: table => new
                {
                    AdviceId = table.Column<Guid>(type: "uuid", nullable: false),
                    TherapistScheduleId = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    AdviceContent = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TherapistAdvice", x => x.AdviceId);
                    table.ForeignKey(
                        name: "FK_TherapistAdvice_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TherapistAdvice_TherapistSchedules_TherapistScheduleId",
                        column: x => x.TherapistScheduleId,
                        principalTable: "TherapistSchedules",
                        principalColumn: "TherapistScheduleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "871908ce-b6ba-4f02-8e33-66b55c3e394e", "AQAAAAIAAYagAAAAECPYYn6xAhPIZxaq6MM43d0mLdoAHYJn0Y/6oX7LLI+vBBrwjf4bwoi4WCnK04HmmA==", "d965aa15-0f1a-46b0-9c71-7fc792ab65c9" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Manager",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f94f6095-4b83-4352-aceb-416aa0244443", "AQAAAAIAAYagAAAAEHogGEme/ntgg1tFsFfbbhE4hxO118Ds8r+QCNBPZ4LHbBNz4BkS7vlWEHChJyBnDw==", "e5bdf26f-c423-4bd1-9180-8ab27b046cc1" });

            migrationBuilder.CreateIndex(
                name: "IX_TherapistAdvice_CustomerId",
                table: "TherapistAdvice",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_TherapistAdvice_TherapistScheduleId",
                table: "TherapistAdvice",
                column: "TherapistScheduleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TherapistAdvice");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "dc06665e-e716-439c-be67-4f0a861aada4", "AQAAAAIAAYagAAAAEDp16/VGJZ7raa8m4AM0Tjs/A1Fvpqd6JDEoErR2l5iALr2+yJv1EHXxG5hRD6rwpA==", "318e9305-a7a0-40a9-8590-d0ba66332eac" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Manager",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a43f6854-48eb-49cd-ae5b-61dc87e8d557", "AQAAAAIAAYagAAAAEGqvZj4ijLOLDGZzD25bx3mQr8YbB32uU9uPNViQzZbdgWkksxESSJUmImQPhRmwHg==", "68b8759d-a33e-437a-8f75-7b28c7eff13e" });
        }
    }
}
