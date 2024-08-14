using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateChildsDevelopmentHistories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChildsDevelopmentHistories",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FeedingHistory = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    SocialSmile = table.Column<short>(type: "Smallint", nullable: false),
                    HeadHolding = table.Column<short>(type: "Smallint", nullable: false),
                    TurnTowardSoundOrigin = table.Column<short>(type: "Smallint", nullable: false),
                    GraspToy = table.Column<short>(type: "Smallint", nullable: false),
                    FollowObjectsWithEyes = table.Column<short>(type: "Smallint", nullable: false),
                    RollsOver = table.Column<short>(type: "Smallint", nullable: false),
                    Babbles = table.Column<short>(type: "Smallint", nullable: false),
                    TakesObjectsToMouth = table.Column<short>(type: "Smallint", nullable: false),
                    RepeatsSyllables = table.Column<short>(type: "Smallint", nullable: false),
                    MovesObjects = table.Column<short>(type: "Smallint", nullable: false),
                    PlaysPeekaBoo = table.Column<short>(type: "Smallint", nullable: false),
                    RespondsToOwnName = table.Column<short>(type: "Smallint", nullable: false),
                    TakesStepsWithSupport = table.Column<short>(type: "Smallint", nullable: false),
                    PicksUpSmallObjects = table.Column<short>(type: "Smallint", nullable: false),
                    ImitatesSimpleGestures = table.Column<short>(type: "Smallint", nullable: false),
                    SaysTwoToThreeWords = table.Column<short>(type: "Smallint", nullable: false),
                    Sitting = table.Column<short>(type: "Smallint", nullable: false),
                    Standing = table.Column<short>(type: "Smallint", nullable: false),
                    Walking = table.Column<short>(type: "Smallint", nullable: false),
                    Talking = table.Column<short>(type: "Smallint", nullable: false),
                    DrinksFromCup = table.Column<short>(type: "Smallint", nullable: false),
                    SaysSevenToTenWords = table.Column<short>(type: "Smallint", nullable: false),
                    PointsToBodyParts = table.Column<short>(type: "Smallint", nullable: false),
                    StartsToRun = table.Column<short>(type: "Smallint", nullable: false),
                    PointsPictureOnRequest = table.Column<short>(type: "Smallint", nullable: false),
                    Sings = table.Column<short>(type: "Smallint", nullable: false),
                    BuildTowerWithBox = table.Column<short>(type: "Smallint", nullable: false),
                    JumpsAndRun = table.Column<short>(type: "Smallint", nullable: false),
                    BeginsToDressAndUndress = table.Column<short>(type: "Smallint", nullable: false),
                    GroupsSimilarObjects = table.Column<short>(type: "Smallint", nullable: false),
                    PlaysWithOtherChildren = table.Column<short>(type: "Smallint", nullable: false),
                    SaysFirstNameAndShortStory = table.Column<short>(type: "Smallint", nullable: false),
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
                    table.PrimaryKey("PK_ChildsDevelopmentHistories", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_ChildsDevelopmentHistories_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChildsDevelopmentHistories_ClientId",
                table: "ChildsDevelopmentHistories",
                column: "ClientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChildsDevelopmentHistories");
        }
    }
}
