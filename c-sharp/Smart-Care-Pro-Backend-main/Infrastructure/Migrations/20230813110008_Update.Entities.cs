using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class UpdateEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Acetones_Partographs_PartographID",
                table: "Acetones");

            migrationBuilder.DropForeignKey(
                name: "FK_BloodPressures_Partographs_PartographID",
                table: "BloodPressures");

            migrationBuilder.DropForeignKey(
                name: "FK_Caregivers_Clients_ClientID",
                table: "Caregivers");

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
                name: "FK_Proteins_Partographs_PartographID",
                table: "Proteins");

            migrationBuilder.DropForeignKey(
                name: "FK_Pulses_Partographs_PartographID",
                table: "Pulses");

            migrationBuilder.DropForeignKey(
                name: "FK_Temperatures_Partographs_PartographID",
                table: "Temperatures");

            migrationBuilder.DropForeignKey(
                name: "FK_Volumes_Partographs_PartographID",
                table: "Volumes");

            migrationBuilder.RenameColumn(
                name: "WardName",
                table: "Wards",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "PartographID",
                table: "Volumes",
                newName: "PartographId");

            migrationBuilder.RenameColumn(
                name: "VolumesID",
                table: "Volumes",
                newName: "Oid");

            migrationBuilder.RenameIndex(
                name: "IX_Volumes_PartographID",
                table: "Volumes",
                newName: "IX_Volumes_PartographId");

            migrationBuilder.RenameColumn(
                name: "Dose",
                table: "VaccineDoses",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "PartographID",
                table: "Temperatures",
                newName: "PartographId");

            migrationBuilder.RenameColumn(
                name: "TemperaturesID",
                table: "Temperatures",
                newName: "TemperatureId");

            migrationBuilder.RenameIndex(
                name: "IX_Temperatures_PartographID",
                table: "Temperatures",
                newName: "IX_Temperatures_PartographId");

            migrationBuilder.RenameColumn(
                name: "DrugName",
                table: "TBDrugs",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "Risk",
                table: "Risks",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "PartographID",
                table: "Pulses",
                newName: "PartographId");

            migrationBuilder.RenameColumn(
                name: "PulseID",
                table: "Pulses",
                newName: "InteractionId");

            migrationBuilder.RenameIndex(
                name: "IX_Pulses_PartographID",
                table: "Pulses",
                newName: "IX_Pulses_PartographId");

            migrationBuilder.RenameColumn(
                name: "PartographID",
                table: "Proteins",
                newName: "PartographId");

            migrationBuilder.RenameColumn(
                name: "ProteinsID",
                table: "Proteins",
                newName: "InteractionId");

            migrationBuilder.RenameIndex(
                name: "IX_Proteins_PartographID",
                table: "Proteins",
                newName: "IX_Proteins_PartographId");

            migrationBuilder.RenameColumn(
                name: "EncounterID",
                table: "Partographs",
                newName: "EncounterId");

            migrationBuilder.RenameColumn(
                name: "AdmissionID",
                table: "Partographs",
                newName: "AdmissionId");

            migrationBuilder.RenameColumn(
                name: "PartographID",
                table: "Partographs",
                newName: "InteractionId");

            migrationBuilder.RenameIndex(
                name: "IX_Partographs_EncounterID",
                table: "Partographs",
                newName: "IX_Partographs_EncounterId");

            migrationBuilder.RenameColumn(
                name: "PartographID",
                table: "PartographDetails",
                newName: "PartographId");

            migrationBuilder.RenameColumn(
                name: "PartographDetailsID",
                table: "PartographDetails",
                newName: "InteractionId");

            migrationBuilder.RenameIndex(
                name: "IX_PartographDetails_PartographID",
                table: "PartographDetails",
                newName: "IX_PartographDetails_PartographId");

            migrationBuilder.RenameColumn(
                name: "PartographID",
                table: "Oxytocins",
                newName: "PartographId");

            migrationBuilder.RenameColumn(
                name: "OxytocinID",
                table: "Oxytocins",
                newName: "InteractionId");

            migrationBuilder.RenameIndex(
                name: "IX_Oxytocins_PartographID",
                table: "Oxytocins",
                newName: "IX_Oxytocins_PartographId");

            migrationBuilder.RenameColumn(
                name: "Diagnosis",
                table: "NTGLevelTwoDiagnoses",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "Diagnosis",
                table: "NTGLevelThreeDiagnoses",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "Diagnosis",
                table: "NTGLevelOneDianoses",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "PartographID",
                table: "Mouldings",
                newName: "PartographId");

            migrationBuilder.RenameColumn(
                name: "MouldingDetails",
                table: "Mouldings",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "MouldingID",
                table: "Mouldings",
                newName: "InteractionId");

            migrationBuilder.RenameIndex(
                name: "IX_Mouldings_PartographID",
                table: "Mouldings",
                newName: "IX_Mouldings_PartographId");

            migrationBuilder.RenameColumn(
                name: "PartographID",
                table: "Medicines",
                newName: "PartographId");

            migrationBuilder.RenameColumn(
                name: "MedicinesName",
                table: "Medicines",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "MedicinesID",
                table: "Medicines",
                newName: "InteractionId");

            migrationBuilder.RenameIndex(
                name: "IX_Medicines_PartographID",
                table: "Medicines",
                newName: "IX_Medicines_PartographId");

            migrationBuilder.RenameColumn(
                name: "BrandName",
                table: "MedicineBrands",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "PartographID",
                table: "Liquors",
                newName: "PartographId");

            migrationBuilder.RenameColumn(
                name: "LiquorDetails",
                table: "Liquors",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "LiquorID",
                table: "Liquors",
                newName: "InteractionId");

            migrationBuilder.RenameIndex(
                name: "IX_Liquors_PartographID",
                table: "Liquors",
                newName: "IX_Liquors_PartographId");

            migrationBuilder.RenameColumn(
                name: "PartographID",
                table: "FetalHeartRates",
                newName: "PartographId");

            migrationBuilder.RenameColumn(
                name: "FetalHeartRateID",
                table: "FetalHeartRates",
                newName: "InteractionId");

            migrationBuilder.RenameIndex(
                name: "IX_FetalHeartRates_PartographID",
                table: "FetalHeartRates",
                newName: "IX_FetalHeartRates_PartographId");

            migrationBuilder.RenameColumn(
                name: "PartographID",
                table: "Drops",
                newName: "PartographId");

            migrationBuilder.RenameColumn(
                name: "DropsID",
                table: "Drops",
                newName: "InteractionId");

            migrationBuilder.RenameIndex(
                name: "IX_Drops_PartographID",
                table: "Drops",
                newName: "IX_Drops_PartographId");

            migrationBuilder.RenameColumn(
                name: "PartographID",
                table: "DescentOfHeads",
                newName: "PartographId");

            migrationBuilder.RenameColumn(
                name: "DescentOfHeadID",
                table: "DescentOfHeads",
                newName: "InteractionId");

            migrationBuilder.RenameIndex(
                name: "IX_DescentOfHeads_PartographID",
                table: "DescentOfHeads",
                newName: "IX_DescentOfHeads_PartographId");

            migrationBuilder.RenameColumn(
                name: "DepartmentName",
                table: "Departments",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "OtherComorbidConditions",
                table: "Covids",
                newName: "OtherComorbiditiesConditions");

            migrationBuilder.RenameColumn(
                name: "CovidComorbidConditions",
                table: "CovidComorbidities",
                newName: "CovidComorbidityConditions");

            migrationBuilder.RenameColumn(
                name: "PartographID",
                table: "Contractions",
                newName: "PartographId");

            migrationBuilder.RenameColumn(
                name: "ContractionsID",
                table: "Contractions",
                newName: "ContractionsId");

            migrationBuilder.RenameIndex(
                name: "IX_Contractions_PartographID",
                table: "Contractions",
                newName: "IX_Contractions_PartographId");

            migrationBuilder.RenameColumn(
                name: "PartographID",
                table: "Cervixes",
                newName: "PartographId");

            migrationBuilder.RenameColumn(
                name: "CervixID",
                table: "Cervixes",
                newName: "CervixId");

            migrationBuilder.RenameIndex(
                name: "IX_Cervixes_PartographID",
                table: "Cervixes",
                newName: "IX_Cervixes_PartographId");

            migrationBuilder.RenameColumn(
                name: "ClientID",
                table: "Caregivers",
                newName: "ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_Caregivers_ClientID",
                table: "Caregivers",
                newName: "IX_Caregivers_ClientId");

            migrationBuilder.RenameColumn(
                name: "PartographID",
                table: "BloodPressures",
                newName: "PartographId");

            migrationBuilder.RenameColumn(
                name: "BloodPressureID",
                table: "BloodPressures",
                newName: "BloodPressureId");

            migrationBuilder.RenameIndex(
                name: "IX_BloodPressures_PartographID",
                table: "BloodPressures",
                newName: "IX_BloodPressures_PartographId");

            migrationBuilder.RenameColumn(
                name: "VitaminAgiven",
                table: "Assessments",
                newName: "VitaminAGiven");

            migrationBuilder.RenameColumn(
                name: "RootingRefelx",
                table: "Assessments",
                newName: "RootingReflex");

            migrationBuilder.RenameColumn(
                name: "DrugName",
                table: "ARTDrugs",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "AllergyName",
                table: "Allergies",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "DrugType",
                table: "AllergicDrugs",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "PartographID",
                table: "Acetones",
                newName: "PartographId");

            migrationBuilder.RenameColumn(
                name: "AcetonesDetails",
                table: "Acetones",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "AcetonesID",
                table: "Acetones",
                newName: "Oid");

            migrationBuilder.RenameIndex(
                name: "IX_Acetones_PartographID",
                table: "Acetones",
                newName: "IX_Acetones_PartographId");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "TestSubTypes",
                type: "nvarchar(90)",
                maxLength: 90,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(90)",
                oldMaxLength: 90,
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "PueperiumOutcome",
                table: "MotherDetails",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<byte>(
                name: "EarlyTerminationReason",
                table: "MotherDetails",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<int>(
                name: "Number",
                table: "MotherDeliverySummaries",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ModuleCode",
                table: "ModuleAccesses",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "SampleQuantity",
                table: "Investigations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "FrequencyIntervals",
                type: "nvarchar(90)",
                maxLength: 90,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(90)",
                oldMaxLength: 90,
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "TBSusceptibleRegimen",
                table: "Dots",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<byte>(
                name: "SusceptiblePTType",
                table: "Dots",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<byte>(
                name: "MDRDRRegimenGroup",
                table: "Dots",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<byte>(
                name: "MDRDRRegimen",
                table: "Dots",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<byte>(
                name: "ReasonClientRefusedForVaccination",
                table: "Covaxes",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<byte>(
                name: "MaritalStatus",
                table: "Clients",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<byte>(
                name: "LastTCResult",
                table: "ChildDetails",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<byte>(
                name: "ChildSex",
                table: "ChildDetails",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<byte>(
                name: "NATResult",
                table: "ChiefComplaints",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<byte>(
                name: "RHSensitivity",
                table: "BloodTransfusionHistories",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<byte>(
                name: "KinBloodGroup",
                table: "BloodTransfusionHistories",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<byte>(
                name: "BloodGroup",
                table: "BloodTransfusionHistories",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<byte>(
                name: "TypeOfDelivery",
                table: "BirthDetails",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<byte>(
                name: "Gender",
                table: "BirthDetails",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<short>(
                name: "NumberOfSexualContacts",
                table: "ARTServices",
                type: "SMALLINT",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "SMALLINT");

            migrationBuilder.AlterColumn<short>(
                name: "NumberOfOtherContacts",
                table: "ARTServices",
                type: "SMALLINT",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "SMALLINT");

            migrationBuilder.AlterColumn<short>(
                name: "NumberOfBiologicalContacts",
                table: "ARTServices",
                type: "SMALLINT",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "SMALLINT");

            migrationBuilder.AddForeignKey(
                name: "FK_Acetones_Partographs_PartographId",
                table: "Acetones",
                column: "PartographId",
                principalTable: "Partographs",
                principalColumn: "InteractionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BloodPressures_Partographs_PartographId",
                table: "BloodPressures",
                column: "PartographId",
                principalTable: "Partographs",
                principalColumn: "InteractionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Caregivers_Clients_ClientId",
                table: "Caregivers",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Oid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cervixes_Partographs_PartographId",
                table: "Cervixes",
                column: "PartographId",
                principalTable: "Partographs",
                principalColumn: "InteractionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contractions_Partographs_PartographId",
                table: "Contractions",
                column: "PartographId",
                principalTable: "Partographs",
                principalColumn: "InteractionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DescentOfHeads_Partographs_PartographId",
                table: "DescentOfHeads",
                column: "PartographId",
                principalTable: "Partographs",
                principalColumn: "InteractionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Drops_Partographs_PartographId",
                table: "Drops",
                column: "PartographId",
                principalTable: "Partographs",
                principalColumn: "InteractionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FetalHeartRates_Partographs_PartographId",
                table: "FetalHeartRates",
                column: "PartographId",
                principalTable: "Partographs",
                principalColumn: "InteractionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Liquors_Partographs_PartographId",
                table: "Liquors",
                column: "PartographId",
                principalTable: "Partographs",
                principalColumn: "InteractionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Medicines_Partographs_PartographId",
                table: "Medicines",
                column: "PartographId",
                principalTable: "Partographs",
                principalColumn: "InteractionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Mouldings_Partographs_PartographId",
                table: "Mouldings",
                column: "PartographId",
                principalTable: "Partographs",
                principalColumn: "InteractionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Oxytocins_Partographs_PartographId",
                table: "Oxytocins",
                column: "PartographId",
                principalTable: "Partographs",
                principalColumn: "InteractionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PartographDetails_Partographs_PartographId",
                table: "PartographDetails",
                column: "PartographId",
                principalTable: "Partographs",
                principalColumn: "InteractionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Partographs_Encounters_EncounterId",
                table: "Partographs",
                column: "EncounterId",
                principalTable: "Encounters",
                principalColumn: "Oid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Proteins_Partographs_PartographId",
                table: "Proteins",
                column: "PartographId",
                principalTable: "Partographs",
                principalColumn: "InteractionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pulses_Partographs_PartographId",
                table: "Pulses",
                column: "PartographId",
                principalTable: "Partographs",
                principalColumn: "InteractionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Temperatures_Partographs_PartographId",
                table: "Temperatures",
                column: "PartographId",
                principalTable: "Partographs",
                principalColumn: "InteractionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Volumes_Partographs_PartographId",
                table: "Volumes",
                column: "PartographId",
                principalTable: "Partographs",
                principalColumn: "InteractionId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Acetones_Partographs_PartographId",
                table: "Acetones");

            migrationBuilder.DropForeignKey(
                name: "FK_BloodPressures_Partographs_PartographId",
                table: "BloodPressures");

            migrationBuilder.DropForeignKey(
                name: "FK_Caregivers_Clients_ClientId",
                table: "Caregivers");

            migrationBuilder.DropForeignKey(
                name: "FK_Cervixes_Partographs_PartographId",
                table: "Cervixes");

            migrationBuilder.DropForeignKey(
                name: "FK_Contractions_Partographs_PartographId",
                table: "Contractions");

            migrationBuilder.DropForeignKey(
                name: "FK_DescentOfHeads_Partographs_PartographId",
                table: "DescentOfHeads");

            migrationBuilder.DropForeignKey(
                name: "FK_Drops_Partographs_PartographId",
                table: "Drops");

            migrationBuilder.DropForeignKey(
                name: "FK_FetalHeartRates_Partographs_PartographId",
                table: "FetalHeartRates");

            migrationBuilder.DropForeignKey(
                name: "FK_Liquors_Partographs_PartographId",
                table: "Liquors");

            migrationBuilder.DropForeignKey(
                name: "FK_Medicines_Partographs_PartographId",
                table: "Medicines");

            migrationBuilder.DropForeignKey(
                name: "FK_Mouldings_Partographs_PartographId",
                table: "Mouldings");

            migrationBuilder.DropForeignKey(
                name: "FK_Oxytocins_Partographs_PartographId",
                table: "Oxytocins");

            migrationBuilder.DropForeignKey(
                name: "FK_PartographDetails_Partographs_PartographId",
                table: "PartographDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Partographs_Encounters_EncounterId",
                table: "Partographs");

            migrationBuilder.DropForeignKey(
                name: "FK_Proteins_Partographs_PartographId",
                table: "Proteins");

            migrationBuilder.DropForeignKey(
                name: "FK_Pulses_Partographs_PartographId",
                table: "Pulses");

            migrationBuilder.DropForeignKey(
                name: "FK_Temperatures_Partographs_PartographId",
                table: "Temperatures");

            migrationBuilder.DropForeignKey(
                name: "FK_Volumes_Partographs_PartographId",
                table: "Volumes");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Wards",
                newName: "WardName");

            migrationBuilder.RenameColumn(
                name: "PartographId",
                table: "Volumes",
                newName: "PartographID");

            migrationBuilder.RenameColumn(
                name: "Oid",
                table: "Volumes",
                newName: "VolumesID");

            migrationBuilder.RenameIndex(
                name: "IX_Volumes_PartographId",
                table: "Volumes",
                newName: "IX_Volumes_PartographID");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "VaccineDoses",
                newName: "Dose");

            migrationBuilder.RenameColumn(
                name: "PartographId",
                table: "Temperatures",
                newName: "PartographID");

            migrationBuilder.RenameColumn(
                name: "TemperatureId",
                table: "Temperatures",
                newName: "TemperaturesID");

            migrationBuilder.RenameIndex(
                name: "IX_Temperatures_PartographId",
                table: "Temperatures",
                newName: "IX_Temperatures_PartographID");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "TBDrugs",
                newName: "DrugName");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Risks",
                newName: "Risk");

            migrationBuilder.RenameColumn(
                name: "PartographId",
                table: "Pulses",
                newName: "PartographID");

            migrationBuilder.RenameColumn(
                name: "InteractionId",
                table: "Pulses",
                newName: "PulseID");

            migrationBuilder.RenameIndex(
                name: "IX_Pulses_PartographId",
                table: "Pulses",
                newName: "IX_Pulses_PartographID");

            migrationBuilder.RenameColumn(
                name: "PartographId",
                table: "Proteins",
                newName: "PartographID");

            migrationBuilder.RenameColumn(
                name: "InteractionId",
                table: "Proteins",
                newName: "ProteinsID");

            migrationBuilder.RenameIndex(
                name: "IX_Proteins_PartographId",
                table: "Proteins",
                newName: "IX_Proteins_PartographID");

            migrationBuilder.RenameColumn(
                name: "EncounterId",
                table: "Partographs",
                newName: "EncounterID");

            migrationBuilder.RenameColumn(
                name: "AdmissionId",
                table: "Partographs",
                newName: "AdmissionID");

            migrationBuilder.RenameColumn(
                name: "InteractionId",
                table: "Partographs",
                newName: "PartographID");

            migrationBuilder.RenameIndex(
                name: "IX_Partographs_EncounterId",
                table: "Partographs",
                newName: "IX_Partographs_EncounterID");

            migrationBuilder.RenameColumn(
                name: "PartographId",
                table: "PartographDetails",
                newName: "PartographID");

            migrationBuilder.RenameColumn(
                name: "InteractionId",
                table: "PartographDetails",
                newName: "PartographDetailsID");

            migrationBuilder.RenameIndex(
                name: "IX_PartographDetails_PartographId",
                table: "PartographDetails",
                newName: "IX_PartographDetails_PartographID");

            migrationBuilder.RenameColumn(
                name: "PartographId",
                table: "Oxytocins",
                newName: "PartographID");

            migrationBuilder.RenameColumn(
                name: "InteractionId",
                table: "Oxytocins",
                newName: "OxytocinID");

            migrationBuilder.RenameIndex(
                name: "IX_Oxytocins_PartographId",
                table: "Oxytocins",
                newName: "IX_Oxytocins_PartographID");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "NTGLevelTwoDiagnoses",
                newName: "Diagnosis");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "NTGLevelThreeDiagnoses",
                newName: "Diagnosis");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "NTGLevelOneDianoses",
                newName: "Diagnosis");

            migrationBuilder.RenameColumn(
                name: "PartographId",
                table: "Mouldings",
                newName: "PartographID");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Mouldings",
                newName: "MouldingDetails");

            migrationBuilder.RenameColumn(
                name: "InteractionId",
                table: "Mouldings",
                newName: "MouldingID");

            migrationBuilder.RenameIndex(
                name: "IX_Mouldings_PartographId",
                table: "Mouldings",
                newName: "IX_Mouldings_PartographID");

            migrationBuilder.RenameColumn(
                name: "PartographId",
                table: "Medicines",
                newName: "PartographID");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Medicines",
                newName: "MedicinesName");

            migrationBuilder.RenameColumn(
                name: "InteractionId",
                table: "Medicines",
                newName: "MedicinesID");

            migrationBuilder.RenameIndex(
                name: "IX_Medicines_PartographId",
                table: "Medicines",
                newName: "IX_Medicines_PartographID");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "MedicineBrands",
                newName: "BrandName");

            migrationBuilder.RenameColumn(
                name: "PartographId",
                table: "Liquors",
                newName: "PartographID");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Liquors",
                newName: "LiquorDetails");

            migrationBuilder.RenameColumn(
                name: "InteractionId",
                table: "Liquors",
                newName: "LiquorID");

            migrationBuilder.RenameIndex(
                name: "IX_Liquors_PartographId",
                table: "Liquors",
                newName: "IX_Liquors_PartographID");

            migrationBuilder.RenameColumn(
                name: "PartographId",
                table: "FetalHeartRates",
                newName: "PartographID");

            migrationBuilder.RenameColumn(
                name: "InteractionId",
                table: "FetalHeartRates",
                newName: "FetalHeartRateID");

            migrationBuilder.RenameIndex(
                name: "IX_FetalHeartRates_PartographId",
                table: "FetalHeartRates",
                newName: "IX_FetalHeartRates_PartographID");

            migrationBuilder.RenameColumn(
                name: "PartographId",
                table: "Drops",
                newName: "PartographID");

            migrationBuilder.RenameColumn(
                name: "InteractionId",
                table: "Drops",
                newName: "DropsID");

            migrationBuilder.RenameIndex(
                name: "IX_Drops_PartographId",
                table: "Drops",
                newName: "IX_Drops_PartographID");

            migrationBuilder.RenameColumn(
                name: "PartographId",
                table: "DescentOfHeads",
                newName: "PartographID");

            migrationBuilder.RenameColumn(
                name: "InteractionId",
                table: "DescentOfHeads",
                newName: "DescentOfHeadID");

            migrationBuilder.RenameIndex(
                name: "IX_DescentOfHeads_PartographId",
                table: "DescentOfHeads",
                newName: "IX_DescentOfHeads_PartographID");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Departments",
                newName: "DepartmentName");

            migrationBuilder.RenameColumn(
                name: "OtherComorbiditiesConditions",
                table: "Covids",
                newName: "OtherComorbidConditions");

            migrationBuilder.RenameColumn(
                name: "CovidComorbidityConditions",
                table: "CovidComorbidities",
                newName: "CovidComorbidConditions");

            migrationBuilder.RenameColumn(
                name: "PartographId",
                table: "Contractions",
                newName: "PartographID");

            migrationBuilder.RenameColumn(
                name: "ContractionsId",
                table: "Contractions",
                newName: "ContractionsID");

            migrationBuilder.RenameIndex(
                name: "IX_Contractions_PartographId",
                table: "Contractions",
                newName: "IX_Contractions_PartographID");

            migrationBuilder.RenameColumn(
                name: "PartographId",
                table: "Cervixes",
                newName: "PartographID");

            migrationBuilder.RenameColumn(
                name: "CervixId",
                table: "Cervixes",
                newName: "CervixID");

            migrationBuilder.RenameIndex(
                name: "IX_Cervixes_PartographId",
                table: "Cervixes",
                newName: "IX_Cervixes_PartographID");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "Caregivers",
                newName: "ClientID");

            migrationBuilder.RenameIndex(
                name: "IX_Caregivers_ClientId",
                table: "Caregivers",
                newName: "IX_Caregivers_ClientID");

            migrationBuilder.RenameColumn(
                name: "PartographId",
                table: "BloodPressures",
                newName: "PartographID");

            migrationBuilder.RenameColumn(
                name: "BloodPressureId",
                table: "BloodPressures",
                newName: "BloodPressureID");

            migrationBuilder.RenameIndex(
                name: "IX_BloodPressures_PartographId",
                table: "BloodPressures",
                newName: "IX_BloodPressures_PartographID");

            migrationBuilder.RenameColumn(
                name: "VitaminAGiven",
                table: "Assessments",
                newName: "VitaminAgiven");

            migrationBuilder.RenameColumn(
                name: "RootingReflex",
                table: "Assessments",
                newName: "RootingRefelx");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "ARTDrugs",
                newName: "DrugName");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Allergies",
                newName: "AllergyName");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "AllergicDrugs",
                newName: "DrugType");

            migrationBuilder.RenameColumn(
                name: "PartographId",
                table: "Acetones",
                newName: "PartographID");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Acetones",
                newName: "AcetonesDetails");

            migrationBuilder.RenameColumn(
                name: "Oid",
                table: "Acetones",
                newName: "AcetonesID");

            migrationBuilder.RenameIndex(
                name: "IX_Acetones_PartographId",
                table: "Acetones",
                newName: "IX_Acetones_PartographID");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "TestSubTypes",
                type: "nvarchar(90)",
                maxLength: 90,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(90)",
                oldMaxLength: 90);

            migrationBuilder.AlterColumn<byte>(
                name: "PueperiumOutcome",
                table: "MotherDetails",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "EarlyTerminationReason",
                table: "MotherDetails",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Number",
                table: "MotherDeliverySummaries",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ModuleCode",
                table: "ModuleAccesses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SampleQuantity",
                table: "Investigations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "FrequencyIntervals",
                type: "nvarchar(90)",
                maxLength: 90,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(90)",
                oldMaxLength: 90);

            migrationBuilder.AlterColumn<byte>(
                name: "TBSusceptibleRegimen",
                table: "Dots",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "SusceptiblePTType",
                table: "Dots",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "MDRDRRegimenGroup",
                table: "Dots",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "MDRDRRegimen",
                table: "Dots",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "ReasonClientRefusedForVaccination",
                table: "Covaxes",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "MaritalStatus",
                table: "Clients",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "LastTCResult",
                table: "ChildDetails",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "ChildSex",
                table: "ChildDetails",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "NATResult",
                table: "ChiefComplaints",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "RHSensitivity",
                table: "BloodTransfusionHistories",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "KinBloodGroup",
                table: "BloodTransfusionHistories",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "BloodGroup",
                table: "BloodTransfusionHistories",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "TypeOfDelivery",
                table: "BirthDetails",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "Gender",
                table: "BirthDetails",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "NumberOfSexualContacts",
                table: "ARTServices",
                type: "SMALLINT",
                nullable: false,
                defaultValue: (short)0,
                oldClrType: typeof(short),
                oldType: "SMALLINT",
                oldNullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "NumberOfOtherContacts",
                table: "ARTServices",
                type: "SMALLINT",
                nullable: false,
                defaultValue: (short)0,
                oldClrType: typeof(short),
                oldType: "SMALLINT",
                oldNullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "NumberOfBiologicalContacts",
                table: "ARTServices",
                type: "SMALLINT",
                nullable: false,
                defaultValue: (short)0,
                oldClrType: typeof(short),
                oldType: "SMALLINT",
                oldNullable: true);

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
                name: "FK_Caregivers_Clients_ClientID",
                table: "Caregivers",
                column: "ClientID",
                principalTable: "Clients",
                principalColumn: "Oid",
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
                name: "FK_Temperatures_Partographs_PartographID",
                table: "Temperatures",
                column: "PartographID",
                principalTable: "Partographs",
                principalColumn: "PartographID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Volumes_Partographs_PartographID",
                table: "Volumes",
                column: "PartographID",
                principalTable: "Partographs",
                principalColumn: "PartographID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
