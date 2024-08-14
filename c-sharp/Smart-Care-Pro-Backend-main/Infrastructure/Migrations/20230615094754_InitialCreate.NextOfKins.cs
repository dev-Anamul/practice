using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateNextOfKins : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NextOfKins",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    NextOfKinType = table.Column<byte>(type: "tinyint", nullable: false),
                    OtherNextOfKinType = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true),
                    HouseNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    StreetName = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true),
                    Township = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true),
                    ChiefName = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    CellphoneCountryCode = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    Cellphone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    OtherCellphoneCountryCode = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    OtherCellphone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    EmailAddress = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_NextOfKins", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_NextOfKins_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NextOfKins_ClientId",
                table: "NextOfKins",
                column: "ClientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NextOfKins");
        }
    }
}
