using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    public partial class TIN_Update_Constraint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CHK_TIN",
                table: "Person");

            migrationBuilder.AddCheckConstraint(
                name: "CHK",
                table: "Person",
                sql: "len([TaxIndentificationNumber]) = 8");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CHK",
                table: "Person");

            migrationBuilder.AddCheckConstraint(
                name: "CHK_TIN",
                table: "Person",
                sql: "len([TaxIndentificationNumber]) = 8");
        }
    }
}
