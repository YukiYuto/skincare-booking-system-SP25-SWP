using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkincareBookingSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MigrationDb_OrderICollectionTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "10f38a09-c6c0-430e-ba54-55057c10709f", "AQAAAAIAAYagAAAAEGbTa4HJ18fchgdzNg/Txp9oIWxsI7ww0MjeEsVIq2rmEr/YpkFkpLhGGbZ3qKkDFw==", "b6bf7c40-93e9-467a-b887-518955b5f622" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Manager",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "499f9d0a-b3b2-4d5f-9290-c684aec2e979", "AQAAAAIAAYagAAAAENtsRlhtcqPJ24KoRN54QHlqXTeu9b7UM9Nvkw0/H8vZdatsLpTR1kk6MUTSfE3tqw==", "7e0f0b5d-a0c3-48f1-8872-3466ee5c54dc" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d7ce6a15-4f1b-436e-a7d4-a9c57d0aff29", "AQAAAAIAAYagAAAAELMkmDGKOcbL35xDeUxNvlBAwYyoUd361FLgsYFBOijXDMvb8BapdH1dBqDxoceH8w==", "57dee1f4-7661-4726-97ad-d7d0909aa693" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Manager",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1a2037e1-be9f-4d59-b376-8da422cda65b", "AQAAAAIAAYagAAAAELoF84tDBQvjY1K6ghwBB8S2PwxHQW4oVdOyCvaKdSvRit56g/D1XudiIQzwRRSJjQ==", "56cf59e0-9133-4568-b6c1-12884f0a43dc" });
        }
    }
}
