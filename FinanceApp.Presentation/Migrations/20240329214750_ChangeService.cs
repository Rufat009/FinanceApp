﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceApp.Presentation.Migrations
{
    /// <inheritdoc />
    public partial class ChangeService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "MonthCost",
                table: "Services",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "UtilityCost",
                table: "Services",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MonthCost",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "UtilityCost",
                table: "Services");
        }
    }
}
