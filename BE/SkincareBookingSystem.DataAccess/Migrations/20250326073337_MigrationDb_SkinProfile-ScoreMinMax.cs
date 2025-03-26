using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkincareBookingSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MigrationDb_SkinProfileScoreMinMax : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Score",
                table: "SkinProfile",
                newName: "ScoreMin");

            migrationBuilder.AddColumn<int>(
                name: "ScoreMax",
                table: "SkinProfile",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e026a5c5-5fe8-4983-93f0-2d1845a62cf0", "AQAAAAIAAYagAAAAEI7BjcxW9wAEav6LG9AMiOwwD6q7HlPyNtCPdPSe3CCQsrHk4WTqKOgcp7wFnDyQag==", "f870ab78-360a-4e13-a547-b64d5d32dccb" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Manager",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5b9a3588-d3f3-47be-85b3-55a71d961c27", "AQAAAAIAAYagAAAAEIPcmXnu60sRhHOHItNpXts7A7unCSNgpIRy6CxxE3SC0yYL3ODvez7g+4RS+3ftGg==", "2bd08bc2-7987-4435-a60f-6ef19a1bb97c" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScoreMax",
                table: "SkinProfile");

            migrationBuilder.RenameColumn(
                name: "ScoreMin",
                table: "SkinProfile",
                newName: "Score");

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
    }
}
