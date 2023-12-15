using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
	public partial class PersonStore_Information : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			string sp_GetAllPersons = @"
CREATE PROCEDURE [dbo].[GetAllPersons_Second]
AS BEGIN
    SELECT PersonId, Name, Email, DataOfBirth, Gender, Country, CountryID, Address, ReceiveNewsLetters  FROM [dbo].[Person]
END
";
			migrationBuilder.Sql(sp_GetAllPersons);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			string sp_GetAllPersons = @"
    DROP PROCEDURE [dbo].[GetAllPersons_Second]
";
			migrationBuilder.Sql(sp_GetAllPersons);

		}
	}
}
