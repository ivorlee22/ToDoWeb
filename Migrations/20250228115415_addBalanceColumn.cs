using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoWeb.Migrations
{
    /// <inheritdoc />
    public partial class addBalanceColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address2",
                table: "Students");

            migrationBuilder.RenameColumn(
                name: "Adress",
                table: "School",
                newName: "Address");

            migrationBuilder.AddColumn<decimal>(
                name: "Balance",
                table: "Students",
                type: "decimal(18,2)", //18 là số tổng số các chữ số, 2 là số các số thập phân
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Students",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Balance",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Students");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "School",
                newName: "Adress");

            migrationBuilder.AddColumn<string>(
                name: "Address2",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
