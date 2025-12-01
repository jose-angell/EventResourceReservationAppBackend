using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventResourceReservationApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _30112025_AddPropNavegationCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByUserId1",
                table: "Categories",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CreatedByUserId",
                table: "Categories",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CreatedByUserId1",
                table: "Categories",
                column: "CreatedByUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_AspNetUsers_CreatedByUserId",
                table: "Categories",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_AspNetUsers_CreatedByUserId1",
                table: "Categories",
                column: "CreatedByUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_AspNetUsers_CreatedByUserId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_AspNetUsers_CreatedByUserId1",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_CreatedByUserId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_CreatedByUserId1",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId1",
                table: "Categories");
        }
    }
}
