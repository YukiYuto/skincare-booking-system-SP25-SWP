using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkincareBookingSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MigrationDb_AddOrderAppointmentRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ddbd404e-a7d2-428f-9c55-53bc2a659542", "AQAAAAIAAYagAAAAED3HGI3sD8TgAzTXpcsSpuBgn2Zfoxg0gWeyxcylimAHB2YGrHPlT58LZq5kxJNN+Q==", "10424970-4ab9-4765-b910-91bcf23df9fe" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Manager",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fdfc587c-be31-44b1-9225-2fd1fc94674d", "AQAAAAIAAYagAAAAEDHAMFoTtkzGEb8qd5tJAGrvaa8hfJz1WISPh2OQ76Z58MLr1F7Qh81E1ESnH9Qp7Q==", "255b04c0-7b04-428e-b539-54b20e6e2a3e" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "542cc928-825b-427f-8296-d3ae49ebdaa9", "AQAAAAIAAYagAAAAEN4KS3jg7hsDzZJOGUyknwiYd2AQpW6YubXNvFk8NvW3RESNT1XlbKIKwULadtYvyg==", "eeb19498-6a15-42fc-99e5-362d409d2934" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Manager",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "db1f7b38-bf2a-4ee1-952f-bf38e3090bba", "AQAAAAIAAYagAAAAEPJdqe7Upqa9B1j3sQNW6bgnNJuxlpUhnMuv0lBrs8JVDIFWLnOdrMi63TLeHnTlMw==", "db1293c6-dabd-4655-b4b1-d2e1555e880e" });
        }
    }
}
