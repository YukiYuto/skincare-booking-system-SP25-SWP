using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkincareBookingSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MigrationDB_UpdateCustomerSkinProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_SkinProfile_SkinProfileId",
                table: "Customers");

            migrationBuilder.AlterColumn<Guid>(
                name: "SkinProfileId",
                table: "Customers",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_SkinProfile_SkinProfileId",
                table: "Customers",
                column: "SkinProfileId",
                principalTable: "SkinProfile",
                principalColumn: "SkinProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_SkinProfile_SkinProfileId",
                table: "Customers");

            migrationBuilder.AlterColumn<Guid>(
                name: "SkinProfileId",
                table: "Customers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_SkinProfile_SkinProfileId",
                table: "Customers",
                column: "SkinProfileId",
                principalTable: "SkinProfile",
                principalColumn: "SkinProfileId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
