using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkincareBookingSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MigrationDb_AddAppointmentNote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Appointments",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "Appointments");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "13c1bcc1-b8d0-41c4-af2e-78cd54155e30", "AQAAAAIAAYagAAAAEClOe8fSAnWDKBiSTpWHEeRIvXa22qJQcDqGyCSaCpxEtUjkA4gU2KAz4vIFEpDsPg==", "09f03bd9-7668-44d2-add4-4d468379e379" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Manager",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9a6bd1c6-1852-4886-aa74-3bcda87bb4fd", "AQAAAAIAAYagAAAAEMGYwOZvTFgHxbFRsycGYacA0uWgduuYO2K2qAfhQGwhzxnLmTdg+ZnH3tuJsfEDHQ==", "61e94526-195f-4be2-8e24-81f4099d3959" });
        }
    }
}
