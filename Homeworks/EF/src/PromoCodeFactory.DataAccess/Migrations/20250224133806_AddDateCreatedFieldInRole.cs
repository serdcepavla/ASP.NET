using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PromoCodeFactory.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddDateCreatedFieldInRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "Roles",
                type: "TEXT",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "PromoCode",
                keyColumn: "Id",
                keyValue: new Guid("321bbf73-c525-47b7-a615-b7ed600c70b7"),
                columns: new[] { "BeginDate", "ServiceInfo" },
                values: new object[] { new DateTime(2025, 2, 23, 16, 38, 4, 62, DateTimeKind.Local).AddTicks(3388), "Промокод на 34%" });

            migrationBuilder.UpdateData(
                table: "PromoCode",
                keyColumn: "Id",
                keyValue: new Guid("f3bb4250-b000-4c52-9238-9558d5820eca"),
                columns: new[] { "BeginDate", "ServiceInfo" },
                values: new object[] { new DateTime(2025, 2, 23, 16, 38, 4, 70, DateTimeKind.Local).AddTicks(422), "Промокод на 93%" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("53729686-a368-4eeb-8bfa-cc69b6050d02"),
                column: "CreateDate",
                value: null);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b0ae7aac-5493-45cd-ad16-87426a5e7665"),
                column: "CreateDate",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "Roles");

            migrationBuilder.UpdateData(
                table: "PromoCode",
                keyColumn: "Id",
                keyValue: new Guid("321bbf73-c525-47b7-a615-b7ed600c70b7"),
                columns: new[] { "BeginDate", "ServiceInfo" },
                values: new object[] { new DateTime(2025, 2, 23, 16, 34, 29, 827, DateTimeKind.Local).AddTicks(5638), "Промокод на 46%" });

            migrationBuilder.UpdateData(
                table: "PromoCode",
                keyColumn: "Id",
                keyValue: new Guid("f3bb4250-b000-4c52-9238-9558d5820eca"),
                columns: new[] { "BeginDate", "ServiceInfo" },
                values: new object[] { new DateTime(2025, 2, 23, 16, 34, 29, 847, DateTimeKind.Local).AddTicks(854), "Промокод на 26%" });
        }
    }
}
