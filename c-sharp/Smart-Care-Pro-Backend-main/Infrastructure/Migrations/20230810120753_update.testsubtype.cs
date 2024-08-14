using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class updatetestsubtype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Acetones_Partograph_PartographID",
                table: "Acetones");

            migrationBuilder.DropForeignKey(
                name: "FK_BloodPressures_Partograph_PartographID",
                table: "BloodPressures");

            migrationBuilder.DropForeignKey(
                name: "FK_Cervixes_Partograph_PartographID",
                table: "Cervixes");

            migrationBuilder.DropForeignKey(
                name: "FK_Contractions_Partograph_PartographID",
                table: "Contractions");

            migrationBuilder.DropForeignKey(
                name: "FK_DescentOfHeads_Partograph_PartographID",
                table: "DescentOfHeads");

            migrationBuilder.DropForeignKey(
                name: "FK_Drops_Partograph_PartographID",
                table: "Drops");

            migrationBuilder.DropForeignKey(
                name: "FK_FetalHeartRates_Partograph_PartographID",
                table: "FetalHeartRates");

            migrationBuilder.DropForeignKey(
                name: "FK_Liquors_Partograph_PartographID",
                table: "Liquors");

            migrationBuilder.DropForeignKey(
                name: "FK_Medicines_Partograph_PartographID",
                table: "Medicines");

            migrationBuilder.DropForeignKey(
                name: "FK_Mouldings_Partograph_PartographID",
                table: "Mouldings");

            migrationBuilder.DropForeignKey(
                name: "FK_Oxytocins_Partograph_PartographID",
                table: "Oxytocins");

            migrationBuilder.DropForeignKey(
                name: "FK_Partograph_Encounters_EncounterID",
                table: "Partograph");

            migrationBuilder.DropForeignKey(
                name: "FK_PartographDetails_Partograph_PartographID",
                table: "PartographDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_PriorARTExposers_Clients_ClientId",
                table: "PriorARTExposers");

            migrationBuilder.DropForeignKey(
                name: "FK_Proteins_Partograph_PartographID",
                table: "Proteins");

            migrationBuilder.DropForeignKey(
                name: "FK_Pulses_Partograph_PartographID",
                table: "Pulses");

            migrationBuilder.DropForeignKey(
                name: "FK_TakenARTDrugs_PriorARTExposers_PriorExposureId",
                table: "TakenARTDrugs");

            migrationBuilder.DropForeignKey(
                name: "FK_Temperatures_Partograph_PartographID",
                table: "Temperatures");

            migrationBuilder.DropForeignKey(
                name: "FK_Tests_TestSubType_SubtypeId",
                table: "Tests");

            migrationBuilder.DropForeignKey(
                name: "FK_TestSubType_TestTypes_TestTypeId",
                table: "TestSubType");

            migrationBuilder.DropForeignKey(
                name: "FK_Volumes_Partograph_PartographID",
                table: "Volumes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TestSubType",
                table: "TestSubType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PriorARTExposers",
                table: "PriorARTExposers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Partograph",
                table: "Partograph");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BirthDetail",
                table: "BirthDetail");

            migrationBuilder.RenameTable(
                name: "TestSubType",
                newName: "TestSubTypes");

            migrationBuilder.RenameTable(
                name: "PriorARTExposers",
                newName: "PriorARTExposures");

            migrationBuilder.RenameTable(
                name: "Partograph",
                newName: "Partographs");

            migrationBuilder.RenameTable(
                name: "BirthDetail",
                newName: "BirthDetails");

            migrationBuilder.RenameIndex(
                name: "IX_TestSubType_TestTypeId",
                table: "TestSubTypes",
                newName: "IX_TestSubTypes_TestTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_PriorARTExposers_ClientId",
                table: "PriorARTExposures",
                newName: "IX_PriorARTExposures_ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_Partograph_EncounterID",
                table: "Partographs",
                newName: "IX_Partographs_EncounterID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TestSubTypes",
                table: "TestSubTypes",
                column: "Oid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PriorARTExposures",
                table: "PriorARTExposures",
                column: "InteractionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Partographs",
                table: "Partographs",
                column: "PartographID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BirthDetails",
                table: "BirthDetails",
                column: "InteractionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Acetones_Partographs_PartographID",
                table: "Acetones",
                column: "PartographID",
                principalTable: "Partographs",
                principalColumn: "PartographID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BloodPressures_Partographs_PartographID",
                table: "BloodPressures",
                column: "PartographID",
                principalTable: "Partographs",
                principalColumn: "PartographID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cervixes_Partographs_PartographID",
                table: "Cervixes",
                column: "PartographID",
                principalTable: "Partographs",
                principalColumn: "PartographID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contractions_Partographs_PartographID",
                table: "Contractions",
                column: "PartographID",
                principalTable: "Partographs",
                principalColumn: "PartographID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DescentOfHeads_Partographs_PartographID",
                table: "DescentOfHeads",
                column: "PartographID",
                principalTable: "Partographs",
                principalColumn: "PartographID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Drops_Partographs_PartographID",
                table: "Drops",
                column: "PartographID",
                principalTable: "Partographs",
                principalColumn: "PartographID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FetalHeartRates_Partographs_PartographID",
                table: "FetalHeartRates",
                column: "PartographID",
                principalTable: "Partographs",
                principalColumn: "PartographID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Liquors_Partographs_PartographID",
                table: "Liquors",
                column: "PartographID",
                principalTable: "Partographs",
                principalColumn: "PartographID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Medicines_Partographs_PartographID",
                table: "Medicines",
                column: "PartographID",
                principalTable: "Partographs",
                principalColumn: "PartographID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Mouldings_Partographs_PartographID",
                table: "Mouldings",
                column: "PartographID",
                principalTable: "Partographs",
                principalColumn: "PartographID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Oxytocins_Partographs_PartographID",
                table: "Oxytocins",
                column: "PartographID",
                principalTable: "Partographs",
                principalColumn: "PartographID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PartographDetails_Partographs_PartographID",
                table: "PartographDetails",
                column: "PartographID",
                principalTable: "Partographs",
                principalColumn: "PartographID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Partographs_Encounters_EncounterID",
                table: "Partographs",
                column: "EncounterID",
                principalTable: "Encounters",
                principalColumn: "Oid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PriorARTExposures_Clients_ClientId",
                table: "PriorARTExposures",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Oid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Proteins_Partographs_PartographID",
                table: "Proteins",
                column: "PartographID",
                principalTable: "Partographs",
                principalColumn: "PartographID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pulses_Partographs_PartographID",
                table: "Pulses",
                column: "PartographID",
                principalTable: "Partographs",
                principalColumn: "PartographID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TakenARTDrugs_PriorARTExposures_PriorExposureId",
                table: "TakenARTDrugs",
                column: "PriorExposureId",
                principalTable: "PriorARTExposures",
                principalColumn: "InteractionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Temperatures_Partographs_PartographID",
                table: "Temperatures",
                column: "PartographID",
                principalTable: "Partographs",
                principalColumn: "PartographID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_TestSubTypes_SubtypeId",
                table: "Tests",
                column: "SubtypeId",
                principalTable: "TestSubTypes",
                principalColumn: "Oid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TestSubTypes_TestTypes_TestTypeId",
                table: "TestSubTypes",
                column: "TestTypeId",
                principalTable: "TestTypes",
                principalColumn: "Oid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Volumes_Partographs_PartographID",
                table: "Volumes",
                column: "PartographID",
                principalTable: "Partographs",
                principalColumn: "PartographID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Acetones_Partographs_PartographID",
                table: "Acetones");

            migrationBuilder.DropForeignKey(
                name: "FK_BloodPressures_Partographs_PartographID",
                table: "BloodPressures");

            migrationBuilder.DropForeignKey(
                name: "FK_Cervixes_Partographs_PartographID",
                table: "Cervixes");

            migrationBuilder.DropForeignKey(
                name: "FK_Contractions_Partographs_PartographID",
                table: "Contractions");

            migrationBuilder.DropForeignKey(
                name: "FK_DescentOfHeads_Partographs_PartographID",
                table: "DescentOfHeads");

            migrationBuilder.DropForeignKey(
                name: "FK_Drops_Partographs_PartographID",
                table: "Drops");

            migrationBuilder.DropForeignKey(
                name: "FK_FetalHeartRates_Partographs_PartographID",
                table: "FetalHeartRates");

            migrationBuilder.DropForeignKey(
                name: "FK_Liquors_Partographs_PartographID",
                table: "Liquors");

            migrationBuilder.DropForeignKey(
                name: "FK_Medicines_Partographs_PartographID",
                table: "Medicines");

            migrationBuilder.DropForeignKey(
                name: "FK_Mouldings_Partographs_PartographID",
                table: "Mouldings");

            migrationBuilder.DropForeignKey(
                name: "FK_Oxytocins_Partographs_PartographID",
                table: "Oxytocins");

            migrationBuilder.DropForeignKey(
                name: "FK_PartographDetails_Partographs_PartographID",
                table: "PartographDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Partographs_Encounters_EncounterID",
                table: "Partographs");

            migrationBuilder.DropForeignKey(
                name: "FK_PriorARTExposures_Clients_ClientId",
                table: "PriorARTExposures");

            migrationBuilder.DropForeignKey(
                name: "FK_Proteins_Partographs_PartographID",
                table: "Proteins");

            migrationBuilder.DropForeignKey(
                name: "FK_Pulses_Partographs_PartographID",
                table: "Pulses");

            migrationBuilder.DropForeignKey(
                name: "FK_TakenARTDrugs_PriorARTExposures_PriorExposureId",
                table: "TakenARTDrugs");

            migrationBuilder.DropForeignKey(
                name: "FK_Temperatures_Partographs_PartographID",
                table: "Temperatures");

            migrationBuilder.DropForeignKey(
                name: "FK_Tests_TestSubTypes_SubtypeId",
                table: "Tests");

            migrationBuilder.DropForeignKey(
                name: "FK_TestSubTypes_TestTypes_TestTypeId",
                table: "TestSubTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_Volumes_Partographs_PartographID",
                table: "Volumes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TestSubTypes",
                table: "TestSubTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PriorARTExposures",
                table: "PriorARTExposures");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Partographs",
                table: "Partographs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BirthDetails",
                table: "BirthDetails");

            migrationBuilder.RenameTable(
                name: "TestSubTypes",
                newName: "TestSubType");

            migrationBuilder.RenameTable(
                name: "PriorARTExposures",
                newName: "PriorARTExposers");

            migrationBuilder.RenameTable(
                name: "Partographs",
                newName: "Partograph");

            migrationBuilder.RenameTable(
                name: "BirthDetails",
                newName: "BirthDetail");

            migrationBuilder.RenameIndex(
                name: "IX_TestSubTypes_TestTypeId",
                table: "TestSubType",
                newName: "IX_TestSubType_TestTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_PriorARTExposures_ClientId",
                table: "PriorARTExposers",
                newName: "IX_PriorARTExposers_ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_Partographs_EncounterID",
                table: "Partograph",
                newName: "IX_Partograph_EncounterID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TestSubType",
                table: "TestSubType",
                column: "Oid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PriorARTExposers",
                table: "PriorARTExposers",
                column: "InteractionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Partograph",
                table: "Partograph",
                column: "PartographID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BirthDetail",
                table: "BirthDetail",
                column: "InteractionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Acetones_Partograph_PartographID",
                table: "Acetones",
                column: "PartographID",
                principalTable: "Partograph",
                principalColumn: "PartographID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BloodPressures_Partograph_PartographID",
                table: "BloodPressures",
                column: "PartographID",
                principalTable: "Partograph",
                principalColumn: "PartographID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cervixes_Partograph_PartographID",
                table: "Cervixes",
                column: "PartographID",
                principalTable: "Partograph",
                principalColumn: "PartographID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contractions_Partograph_PartographID",
                table: "Contractions",
                column: "PartographID",
                principalTable: "Partograph",
                principalColumn: "PartographID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DescentOfHeads_Partograph_PartographID",
                table: "DescentOfHeads",
                column: "PartographID",
                principalTable: "Partograph",
                principalColumn: "PartographID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Drops_Partograph_PartographID",
                table: "Drops",
                column: "PartographID",
                principalTable: "Partograph",
                principalColumn: "PartographID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FetalHeartRates_Partograph_PartographID",
                table: "FetalHeartRates",
                column: "PartographID",
                principalTable: "Partograph",
                principalColumn: "PartographID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Liquors_Partograph_PartographID",
                table: "Liquors",
                column: "PartographID",
                principalTable: "Partograph",
                principalColumn: "PartographID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Medicines_Partograph_PartographID",
                table: "Medicines",
                column: "PartographID",
                principalTable: "Partograph",
                principalColumn: "PartographID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Mouldings_Partograph_PartographID",
                table: "Mouldings",
                column: "PartographID",
                principalTable: "Partograph",
                principalColumn: "PartographID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Oxytocins_Partograph_PartographID",
                table: "Oxytocins",
                column: "PartographID",
                principalTable: "Partograph",
                principalColumn: "PartographID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Partograph_Encounters_EncounterID",
                table: "Partograph",
                column: "EncounterID",
                principalTable: "Encounters",
                principalColumn: "Oid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PartographDetails_Partograph_PartographID",
                table: "PartographDetails",
                column: "PartographID",
                principalTable: "Partograph",
                principalColumn: "PartographID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PriorARTExposers_Clients_ClientId",
                table: "PriorARTExposers",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Oid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Proteins_Partograph_PartographID",
                table: "Proteins",
                column: "PartographID",
                principalTable: "Partograph",
                principalColumn: "PartographID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pulses_Partograph_PartographID",
                table: "Pulses",
                column: "PartographID",
                principalTable: "Partograph",
                principalColumn: "PartographID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TakenARTDrugs_PriorARTExposers_PriorExposureId",
                table: "TakenARTDrugs",
                column: "PriorExposureId",
                principalTable: "PriorARTExposers",
                principalColumn: "InteractionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Temperatures_Partograph_PartographID",
                table: "Temperatures",
                column: "PartographID",
                principalTable: "Partograph",
                principalColumn: "PartographID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_TestSubType_SubtypeId",
                table: "Tests",
                column: "SubtypeId",
                principalTable: "TestSubType",
                principalColumn: "Oid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TestSubType_TestTypes_TestTypeId",
                table: "TestSubType",
                column: "TestTypeId",
                principalTable: "TestTypes",
                principalColumn: "Oid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Volumes_Partograph_PartographID",
                table: "Volumes",
                column: "PartographID",
                principalTable: "Partograph",
                principalColumn: "PartographID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
