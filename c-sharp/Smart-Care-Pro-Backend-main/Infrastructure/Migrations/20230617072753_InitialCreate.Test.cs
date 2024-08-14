using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateTest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Investigations_Test_TestId",
                table: "Investigations");

            migrationBuilder.DropForeignKey(
                name: "FK_MeasuringUnits_Test_TestId",
                table: "MeasuringUnits");

            migrationBuilder.DropForeignKey(
                name: "FK_ResultOptions_Test_TestId",
                table: "ResultOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Test_TestSubType_SubtypeId",
                table: "Test");

            migrationBuilder.DropForeignKey(
                name: "FK_TestItems_Test_TestId",
                table: "TestItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Test",
                table: "Test");

            migrationBuilder.RenameTable(
                name: "Test",
                newName: "Tests");

            migrationBuilder.RenameIndex(
                name: "IX_Test_SubtypeId",
                table: "Tests",
                newName: "IX_Tests_SubtypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tests",
                table: "Tests",
                column: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_Investigations_Tests_TestId",
                table: "Investigations",
                column: "TestId",
                principalTable: "Tests",
                principalColumn: "Oid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MeasuringUnits_Tests_TestId",
                table: "MeasuringUnits",
                column: "TestId",
                principalTable: "Tests",
                principalColumn: "Oid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ResultOptions_Tests_TestId",
                table: "ResultOptions",
                column: "TestId",
                principalTable: "Tests",
                principalColumn: "Oid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TestItems_Tests_TestId",
                table: "TestItems",
                column: "TestId",
                principalTable: "Tests",
                principalColumn: "Oid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_TestSubType_SubtypeId",
                table: "Tests",
                column: "SubtypeId",
                principalTable: "TestSubType",
                principalColumn: "Oid",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Investigations_Tests_TestId",
                table: "Investigations");

            migrationBuilder.DropForeignKey(
                name: "FK_MeasuringUnits_Tests_TestId",
                table: "MeasuringUnits");

            migrationBuilder.DropForeignKey(
                name: "FK_ResultOptions_Tests_TestId",
                table: "ResultOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_TestItems_Tests_TestId",
                table: "TestItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Tests_TestSubType_SubtypeId",
                table: "Tests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tests",
                table: "Tests");

            migrationBuilder.RenameTable(
                name: "Tests",
                newName: "Test");

            migrationBuilder.RenameIndex(
                name: "IX_Tests_SubtypeId",
                table: "Test",
                newName: "IX_Test_SubtypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Test",
                table: "Test",
                column: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_Investigations_Test_TestId",
                table: "Investigations",
                column: "TestId",
                principalTable: "Test",
                principalColumn: "Oid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MeasuringUnits_Test_TestId",
                table: "MeasuringUnits",
                column: "TestId",
                principalTable: "Test",
                principalColumn: "Oid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ResultOptions_Test_TestId",
                table: "ResultOptions",
                column: "TestId",
                principalTable: "Test",
                principalColumn: "Oid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Test_TestSubType_SubtypeId",
                table: "Test",
                column: "SubtypeId",
                principalTable: "TestSubType",
                principalColumn: "Oid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TestItems_Test_TestId",
                table: "TestItems",
                column: "TestId",
                principalTable: "Test",
                principalColumn: "Oid",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
