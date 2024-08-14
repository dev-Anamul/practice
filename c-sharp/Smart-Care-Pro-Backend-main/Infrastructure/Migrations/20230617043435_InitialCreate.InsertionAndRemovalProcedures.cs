using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateInsertionAndRemovalProcedures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InsertionAndRemovalProcedures",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImplantInsertion = table.Column<bool>(type: "bit", nullable: false),
                    ImplantInsertionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ImplantRemoval = table.Column<bool>(type: "bit", nullable: false),
                    ImplantRemovalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IUCDInsertion = table.Column<bool>(type: "bit", nullable: false),
                    IUCDInsertionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IUCDRemoval = table.Column<bool>(type: "bit", nullable: false),
                    IUCDRemovalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    table.PrimaryKey("PK_InsertionAndRemovalProcedures", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_InsertionAndRemovalProcedures_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InsertionAndRemovalProcedures_ClientId",
                table: "InsertionAndRemovalProcedures",
                column: "ClientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InsertionAndRemovalProcedures");
        }
    }
}
