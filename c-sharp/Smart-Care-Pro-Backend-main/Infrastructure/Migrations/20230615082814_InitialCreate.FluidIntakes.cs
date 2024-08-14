﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateFluidIntakes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FluidIntakes",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IntakeTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    IntakeType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IntakeAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Route = table.Column<byte>(type: "tinyint", nullable: false),
                    FluidId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EncounterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EncounterType = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedIn = table.Column<int>(type: "int", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedIn = table.Column<int>(type: "int", nullable: true),
                    DateModified = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsSynced = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FluidIntakes", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_FluidIntakes_Fluids_FluidId",
                        column: x => x.FluidId,
                        principalTable: "Fluids",
                        principalColumn: "InteractionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FluidIntakes_FluidId",
                table: "FluidIntakes",
                column: "FluidId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FluidIntakes");
        }
    }
}
