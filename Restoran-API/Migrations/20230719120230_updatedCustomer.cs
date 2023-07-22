using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurant_Reservation_Management_System_Api.Migrations
{
    /// <inheritdoc />
    public partial class updatedCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Customers_CustomerId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_CustomerId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Reservations");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "79150917-0447-4005-94de-5a0202b39e4f", "AQAAAAIAAYagAAAAEJk4n9bH3nxAeTS4zyorDkczRQp4MbcY1gagGdonjAzNF0XwqB6COf6BTjz39rZGEw==", "288ad8f1-7f8b-4262-a5d9-b666cb6961aa" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "Reservations",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8cc1a7c8-6777-436b-914c-e8eb0d41b51b", "AQAAAAIAAYagAAAAELtgIWvV5OQ8wyl0bKP9Y/v3WfaGi8+J3mwZpTfZZQ6GuOaZD6F3n6lsol9obr3lHA==", "6ae1a053-791a-4e62-9185-b4624107121d" });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_CustomerId",
                table: "Reservations",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Customers_CustomerId",
                table: "Reservations",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId");
        }
    }
}
