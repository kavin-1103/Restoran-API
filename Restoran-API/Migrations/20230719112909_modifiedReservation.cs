using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurant_Reservation_Management_System_Api.Migrations
{
    /// <inheritdoc />
    public partial class modifiedReservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_AspNetUsers_ApplicationUserId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Customers_CustomerId",
                table: "Reservations");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "Reservations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Reservations",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8cc1a7c8-6777-436b-914c-e8eb0d41b51b", "AQAAAAIAAYagAAAAELtgIWvV5OQ8wyl0bKP9Y/v3WfaGi8+J3mwZpTfZZQ6GuOaZD6F3n6lsol9obr3lHA==", "6ae1a053-791a-4e62-9185-b4624107121d" });

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_AspNetUsers_ApplicationUserId",
                table: "Reservations",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Customers_CustomerId",
                table: "Reservations",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_AspNetUsers_ApplicationUserId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Customers_CustomerId",
                table: "Reservations");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Reservations",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "62e1b2fb-204c-45d0-88e7-b8883c02f134", "AQAAAAIAAYagAAAAEBZoNaO69cGLZ6XaRz2kDaNne/8qXOX/KETtlqTg1gHfm/wcBa/XMdvRHiwp+y4S4w==", "540b4917-f683-462f-ab0e-e2db48a8595c" });

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_AspNetUsers_ApplicationUserId",
                table: "Reservations",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Customers_CustomerId",
                table: "Reservations",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
