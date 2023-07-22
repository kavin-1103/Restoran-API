using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurant_Reservation_Management_System_Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedResetPassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsVerified",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Otp",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "OtpExpiration",
                table: "AspNetUsers",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OtpResendCount",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "IsVerified", "Otp", "OtpExpiration", "OtpResendCount", "PasswordHash", "SecurityStamp" },
                values: new object[] { "62e1b2fb-204c-45d0-88e7-b8883c02f134", false, null, null, 0, "AQAAAAIAAYagAAAAEBZoNaO69cGLZ6XaRz2kDaNne/8qXOX/KETtlqTg1gHfm/wcBa/XMdvRHiwp+y4S4w==", "540b4917-f683-462f-ab0e-e2db48a8595c" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVerified",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Otp",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "OtpExpiration",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "OtpResendCount",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b53f1020-897c-40b1-9b27-22a316896c21", "AQAAAAIAAYagAAAAEKPnakKHlpE8PZhBIhw2V9DwUzXObKKgvhyzVVUF4s8wNjVOJEJ2ws1DSV/KInD77A==", "16d1a9c6-73f4-46b0-8292-147c83fc78ed" });
        }
    }
}
