using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class UpdatePastMedicalCondition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalConditions_PastMedicalConditons_PastMedicalConditonId",
                table: "MedicalConditions");

            migrationBuilder.DropTable(
                name: "PastMedicalConditons");

            migrationBuilder.CreateTable(
                name: "PastMedicalConditions",
                columns: table => new
                {
                    Oid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: false),
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
                    table.PrimaryKey("PK_PastMedicalConditions", x => x.Oid);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalConditions_PastMedicalConditions_PastMedicalConditonId",
                table: "MedicalConditions",
                column: "PastMedicalConditonId",
                principalTable: "PastMedicalConditions",
                principalColumn: "Oid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalConditions_PastMedicalConditions_PastMedicalConditonId",
                table: "MedicalConditions");

            migrationBuilder.DropTable(
                name: "PastMedicalConditions");

            migrationBuilder.CreateTable(
                name: "PastMedicalConditons",
                columns: table => new
                {
                    Oid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedIn = table.Column<int>(type: "int", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    DateModified = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsSynced = table.Column<bool>(type: "bit", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedIn = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PastMedicalConditons", x => x.Oid);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalConditions_PastMedicalConditons_PastMedicalConditonId",
                table: "MedicalConditions",
                column: "PastMedicalConditonId",
                principalTable: "PastMedicalConditons",
                principalColumn: "Oid");
        }
    }
}
