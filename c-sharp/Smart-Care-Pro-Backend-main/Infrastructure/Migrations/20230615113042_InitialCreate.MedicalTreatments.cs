﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateMedicalTreatments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MedicalTreatments",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Treatments = table.Column<byte>(type: "tinyint", nullable: true),
                    Other = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true),
                    DeliveryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_MedicalTreatments", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_MedicalTreatments_MotherDeliverySummaries_DeliveryId",
                        column: x => x.DeliveryId,
                        principalTable: "MotherDeliverySummaries",
                        principalColumn: "InteractionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MedicalTreatments_DeliveryId",
                table: "MedicalTreatments",
                column: "DeliveryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedicalTreatments");
        }
    }
}
