using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class UpdateTakenARTDrugs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TakenARTDrugs_PriorARTExposers_PriorExposerId",
                table: "TakenARTDrugs");

            migrationBuilder.DropIndex(
                name: "IX_TakenARTDrugs_PriorExposerId",
                table: "TakenARTDrugs");

            migrationBuilder.DropColumn(
                name: "PriorExposerId",
                table: "TakenARTDrugs");

            migrationBuilder.CreateIndex(
                name: "IX_TakenARTDrugs_PriorExposureId",
                table: "TakenARTDrugs",
                column: "PriorExposureId");

            migrationBuilder.AddForeignKey(
                name: "FK_TakenARTDrugs_PriorARTExposers_PriorExposureId",
                table: "TakenARTDrugs",
                column: "PriorExposureId",
                principalTable: "PriorARTExposers",
                principalColumn: "InteractionId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TakenARTDrugs_PriorARTExposers_PriorExposureId",
                table: "TakenARTDrugs");

            migrationBuilder.DropIndex(
                name: "IX_TakenARTDrugs_PriorExposureId",
                table: "TakenARTDrugs");

            migrationBuilder.AddColumn<Guid>(
                name: "PriorExposerId",
                table: "TakenARTDrugs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TakenARTDrugs_PriorExposerId",
                table: "TakenARTDrugs",
                column: "PriorExposerId");

            migrationBuilder.AddForeignKey(
                name: "FK_TakenARTDrugs_PriorARTExposers_PriorExposerId",
                table: "TakenARTDrugs",
                column: "PriorExposerId",
                principalTable: "PriorARTExposers",
                principalColumn: "InteractionId");
        }
    }
}
