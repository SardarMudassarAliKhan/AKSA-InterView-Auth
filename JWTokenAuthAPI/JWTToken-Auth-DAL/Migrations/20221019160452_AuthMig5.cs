using Microsoft.EntityFrameworkCore.Migrations;

namespace JWTToken_Auth_DAL.Migrations
{
    public partial class AuthMig5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LogInId",
                table: "Departments",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogInId",
                table: "Departments");
        }
    }
}
