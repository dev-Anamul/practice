using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateUserAccounts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserAccounts",
                columns: table => new
                {
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    DOB = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    Sex = table.Column<byte>(type: "tinyint", nullable: false),
                    Designation = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    NRC = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    NoNRC = table.Column<bool>(type: "bit", nullable: false),
                    ContactAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CountryCode = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    Cellphone = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    Username = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserType = table.Column<byte>(type: "tinyint", nullable: false),
                    IsAccountActive = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_UserAccounts", x => x.Oid);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAccounts");
        }
    }
}
