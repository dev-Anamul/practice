using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateAssessments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Assessments",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GeneralCondition = table.Column<byte>(type: "tinyint", nullable: false),
                    NutritionalStatus = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true),
                    JVP = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true),
                    HydrationStatus = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true),
                    Glucose = table.Column<bool>(type: "bit", nullable: false),
                    Scoring = table.Column<byte>(type: "tinyint", nullable: true),
                    VaricoseVein = table.Column<byte>(type: "tinyint", nullable: true),
                    Albumin = table.Column<byte>(type: "tinyint", nullable: true),
                    UrineOutput = table.Column<byte>(type: "tinyint", nullable: true),
                    Feeding = table.Column<byte>(type: "tinyint", nullable: true),
                    PassedMeconium = table.Column<byte>(type: "tinyint", nullable: true),
                    UrinePassed = table.Column<byte>(type: "tinyint", nullable: true),
                    ChildCardIssued = table.Column<byte>(type: "tinyint", nullable: true),
                    VitaminAgiven = table.Column<byte>(type: "tinyint", nullable: true),
                    FatherLiving = table.Column<byte>(type: "tinyint", nullable: true),
                    MotherLiving = table.Column<byte>(type: "tinyint", nullable: true),
                    Pallor = table.Column<byte>(type: "tinyint", nullable: true),
                    Edema = table.Column<byte>(type: "tinyint", nullable: true),
                    Clubbing = table.Column<byte>(type: "tinyint", nullable: true),
                    Jaundice = table.Column<byte>(type: "tinyint", nullable: true),
                    Cyanosis = table.Column<byte>(type: "tinyint", nullable: true),
                    Hb = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PuerperalCondition = table.Column<byte>(type: "tinyint", nullable: true),
                    BreastFeeding = table.Column<byte>(type: "tinyint", nullable: true),
                    InvolutionOfUterus = table.Column<byte>(type: "tinyint", nullable: true),
                    Lochia = table.Column<byte>(type: "tinyint", nullable: true),
                    PerineumCondition = table.Column<byte>(type: "tinyint", nullable: true),
                    EpisiotomyCondition = table.Column<byte>(type: "tinyint", nullable: true),
                    AdditionalNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fontanelles = table.Column<byte>(type: "tinyint", nullable: true),
                    Skull = table.Column<byte>(type: "tinyint", nullable: true),
                    Eyes = table.Column<byte>(type: "tinyint", nullable: true),
                    Mouth = table.Column<byte>(type: "tinyint", nullable: true),
                    Chest = table.Column<byte>(type: "tinyint", nullable: true),
                    Back = table.Column<byte>(type: "tinyint", nullable: true),
                    Limbs = table.Column<byte>(type: "tinyint", nullable: true),
                    Genitals = table.Column<byte>(type: "tinyint", nullable: true),
                    SymmetricalMoroReaction = table.Column<byte>(type: "tinyint", nullable: true),
                    MoroReaction = table.Column<byte>(type: "tinyint", nullable: true),
                    IsGoodGraspReflex = table.Column<bool>(type: "bit", nullable: false),
                    IsMeconiumPassed = table.Column<bool>(type: "bit", nullable: false),
                    IsGoodHeadControl = table.Column<bool>(type: "bit", nullable: false),
                    OrtolaniSign = table.Column<byte>(type: "tinyint", nullable: true),
                    RootingRefelx = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true),
                    SuckingReflex = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true),
                    PalmarReflex = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true),
                    PlantarGrasp = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true),
                    SteppingReflex = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true),
                    GalantReflex = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true),
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
                    table.PrimaryKey("PK_Assessments", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_Assessments_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_ClientId",
                table: "Assessments",
                column: "ClientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Assessments");
        }
    }
}
