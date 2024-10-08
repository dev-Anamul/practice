﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateObstetricExaminations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ObstericExaminations",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SFH = table.Column<int>(type: "int", nullable: false),
                    Presentation = table.Column<byte>(type: "tinyint", nullable: false),
                    Engaged = table.Column<byte>(type: "tinyint", nullable: false),
                    Lie = table.Column<byte>(type: "tinyint", nullable: false),
                    FetalHeart = table.Column<byte>(type: "tinyint", nullable: false),
                    FetalHeartRate = table.Column<int>(type: "int", nullable: false),
                    Contraction = table.Column<byte>(type: "tinyint", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_ObstericExaminations", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_ObstericExaminations_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ObstericExaminations_ClientId",
                table: "ObstericExaminations",
                column: "ClientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ObstericExaminations");
        }
    }
}
