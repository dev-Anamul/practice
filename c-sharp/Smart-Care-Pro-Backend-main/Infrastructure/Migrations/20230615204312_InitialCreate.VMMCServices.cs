using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateVMMCServices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VMMCServices",
                columns: table => new
                {
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsConsentGiven = table.Column<bool>(type: "bit", nullable: false),
                    MCNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PresentedHIVStatus = table.Column<byte>(type: "tinyint", nullable: true),
                    HIVStatusEvidencePresented = table.Column<bool>(type: "bit", nullable: false),
                    IsPreTestCounsellingOffered = table.Column<bool>(type: "bit", nullable: false),
                    IsHIVTestingServiceOffered = table.Column<bool>(type: "bit", nullable: false),
                    IsPostTestCounsellingOffered = table.Column<bool>(type: "bit", nullable: false),
                    IsReferredToARTIfPositive = table.Column<bool>(type: "bit", nullable: false),
                    HasDentures = table.Column<bool>(type: "bit", nullable: false),
                    HasLooseTeeth = table.Column<bool>(type: "bit", nullable: false),
                    HasAbnormalitiesOfTheNeck = table.Column<bool>(type: "bit", nullable: false),
                    MandibleSize = table.Column<byte>(type: "tinyint", nullable: false),
                    TongueSize = table.Column<byte>(type: "tinyint", nullable: false),
                    InterincisorGap = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MovementOfHeadNeck = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ThyromentalDistance = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AtlantoOccipitalFlexion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MallampatiClass = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ASAClassification = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IVAccess = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BonyLandmarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
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
                    table.PrimaryKey("PK_VMMCServices", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_VMMCServices_Clients_Oid",
                        column: x => x.Oid,
                        principalTable: "Clients",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VMMCServices");
        }
    }
}
