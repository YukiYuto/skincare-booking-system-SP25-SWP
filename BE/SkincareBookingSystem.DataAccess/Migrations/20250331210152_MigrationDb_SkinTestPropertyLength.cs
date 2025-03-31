using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkincareBookingSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MigrationDb_SkinTestPropertyLength : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "TestQuestion",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "TestQuestion",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "TestAnswer",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "TestQuestion",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "TestQuestion",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "TestAnswer",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6d521d75-0c58-4c1c-939d-d1448d731ea8", "AQAAAAIAAYagAAAAEO9dq9oXAWwnG+VNmxmpOPJ1Y5RPp+YXxLRhcVwK5vEMgeh1nP4koDvPkYx02pacYQ==", "17897c4f-09e7-4e92-9b49-850c0962249e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SkinBookingSystem-Manager",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c09d7ced-1a40-4807-b3ef-844fad0d71bc", "AQAAAAIAAYagAAAAEPdozSa5EKXba9VH/v1m/sqST5WjEqrOn+wQysS0DqiPS7x+6KJTI9Ps1FXVNEAF8g==", "52b93042-fd07-4f0c-89ab-01ee5e59094d" });
        }
    }
}
