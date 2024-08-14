using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateOptedCircumcisionReasons : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OptedCircumcisionReasons",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CircumcisionReasonId = table.Column<int>(type: "int", nullable: false),
                    VMMCServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_OptedCircumcisionReasons", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_OptedCircumcisionReasons_CircumcisionReasons_CircumcisionReasonId",
                        column: x => x.CircumcisionReasonId,
                        principalTable: "CircumcisionReasons",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OptedCircumcisionReasons_VMMCServices_VMMCServiceId",
                        column: x => x.VMMCServiceId,
                        principalTable: "VMMCServices",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OptedCircumcisionReasons_CircumcisionReasonId",
                table: "OptedCircumcisionReasons",
                column: "CircumcisionReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_OptedCircumcisionReasons_VMMCServiceId",
                table: "OptedCircumcisionReasons",
                column: "VMMCServiceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OptedCircumcisionReasons");
        }
    }
}
