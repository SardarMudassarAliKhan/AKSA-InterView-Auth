using Microsoft.EntityFrameworkCore.Migrations;

namespace JWTToken_Auth_DAL.Migrations
{
    public partial class AuthMig4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhotoFileName",
                table: "Employees",
                newName: "LoginId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LoginId",
                table: "Employees",
                newName: "PhotoFileName");
        }
    }
}
