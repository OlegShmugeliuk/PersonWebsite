using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    public partial class InsertPerson_StoredProcedure : Migration
    {
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			string sp_GetAllPersons = @"
CREATE PROCEDURE [dbo].[InsertPerson]
(@PersonId uniqueidentifier, @Name nvarchar(40), @Email nvarchar(40), @DataOfBirth datetime2(7), @Gender varchar(10),
@Country varchar(10), @CountryID uniqueidentifier, @Address nvarchar(1000), @ReceiveNewsLetters bit)
AS BEGIN
    INSERT INTO [dbo].[Person](PersonId, Name, Email, DataOfBirth, Gender, Country, CountryID, Address, ReceiveNewsLetters) VALUES (@PersonId, @Name, @Email, @DataOfBirth, @Gender, @Country, @CountryID, @Address, @ReceiveNewsLetters)
END
";
			migrationBuilder.Sql(sp_GetAllPersons);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			string sp_GetAllPersons = @"
    DROP PROCEDURE [dbo].[InsertPerson]
";
			migrationBuilder.Sql(sp_GetAllPersons);

		}
	
}
}
