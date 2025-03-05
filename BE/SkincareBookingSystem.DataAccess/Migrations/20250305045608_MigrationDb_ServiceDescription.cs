using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkincareBookingSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MigrationDb_ServiceDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Services",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8daaf1e2-c105-459a-b497-fc8dc042d842", "AQAAAAIAAYagAAAAEOdxW8rxmZ8L+Zhh+9a4BXmq33OcBUrKSgtRlRuY0USgKIRSOun86+6MKl92jToX0A==", "34c73459-fbf7-4b7d-84ed-7176af5c317f" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Manager",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3b42ab95-3ca1-40df-803d-6b8d73415112", "AQAAAAIAAYagAAAAEDo/MNAqoLVcDRbZ7/tx9RMT1YxqnklTrVN1DqVf+wBUFZDXoaZ1g7aRJfKqTf7dYg==", "e30e32c5-c388-4e8a-bf9e-de3787c09c93" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Services",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "296fb191-aea8-44c2-b120-1b2f79345b45", "AQAAAAIAAYagAAAAEJlezUqUljEFcuhkB5s8Ugt/gelDwSbYMUFbKQOhmv/q/HJaNdw+xEwzxLfzWxEj/Q==", "4e57e6f8-c059-48c8-816e-1f267721e19f" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Manager",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5cbe70a1-2232-4c7b-b4f6-37ccec1910df", "AQAAAAIAAYagAAAAEDqoBKGC99eun0A75QzpaTFcMv5fIM8VuhqY6ZywK38RHlZGmgVOTaXAgQQFC9j8fA==", "b16271ef-9bb6-4d00-af79-227a1be40a38" });
        }
    }
}
