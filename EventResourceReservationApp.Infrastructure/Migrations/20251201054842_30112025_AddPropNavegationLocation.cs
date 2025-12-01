using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventResourceReservationApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _30112025_AddPropNavegationLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByUserId1",
                table: "Locations",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Locations_CreatedByUserId",
                table: "Locations",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_CreatedByUserId1",
                table: "Locations",
                column: "CreatedByUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_AspNetUsers_CreatedByUserId",
                table: "Locations",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_AspNetUsers_CreatedByUserId1",
                table: "Locations",
                column: "CreatedByUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_AspNetUsers_CreatedByUserId",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_AspNetUsers_CreatedByUserId1",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Locations_CreatedByUserId",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Locations_CreatedByUserId1",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId1",
                table: "Locations");
        }
    }
}
