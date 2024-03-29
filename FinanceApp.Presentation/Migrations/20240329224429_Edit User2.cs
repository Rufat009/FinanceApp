using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceApp.Presentation.Migrations
{
    /// <inheritdoc />
    public partial class EditUser2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AbonentNumber",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AbonentNumber",
                table: "AspNetUsers");
        }
    }
}
