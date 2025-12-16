using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventResourceReservationApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _15122025__InitialReservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReservationCarItems_Reservations_ReservationId",
                table: "ReservationCarItems");

            migrationBuilder.DropIndex(
                name: "IX_ReservationCarItems_ReservationId",
                table: "ReservationCarItems");

            migrationBuilder.DropColumn(
                name: "ReservationId",
                table: "ReservationCarItems");

            migrationBuilder.AddColumn<Guid>(
                name: "ResourceId",
                table: "Reservations",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_ResourceId",
                table: "Reservations",
                column: "ResourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Resources_ResourceId",
                table: "Reservations",
                column: "ResourceId",
                principalTable: "Resources",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Resources_ResourceId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_ResourceId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "ResourceId",
                table: "Reservations");

            migrationBuilder.AddColumn<Guid>(
                name: "ReservationId",
                table: "ReservationCarItems",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReservationCarItems_ReservationId",
                table: "ReservationCarItems",
                column: "ReservationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReservationCarItems_Reservations_ReservationId",
                table: "ReservationCarItems",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id");
        }
    }
}
