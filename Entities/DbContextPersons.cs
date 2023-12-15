using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
	public class DbContextPersons:DbContext
	{
		public DbContextPersons(DbContextOptions option):base(option) { 
			
		}
		public DbSet<Person> Persons { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<Person>().ToTable("Person");

			string PersonJson = System.IO.File.ReadAllText("persons.json");
			List<Person> persons = System.Text.Json.JsonSerializer.Deserialize<List<Person>>(PersonJson);
			foreach (Person person in persons)
			{
				modelBuilder.Entity<Person>().HasData(person);				
			}
			//Fluent API (вільний API)
			modelBuilder.Entity<Person>().Property(temp => temp.TIN)
				.HasColumnName("TaxIndentificationNumber")
				.HasColumnType("varchar(8)")
				.HasDefaultValue("ABCD");

			//modelBuilder.Entity<Person>().HasIndex(temp => temp.TIN).IsUnique();

			//перевіряє обмеження 
			//modelBuilder.Entity<Person>().HasCheckConstraint("CHK", "len([TaxIndentificationNumber]) = 8");
			//modelBuilder.Entity<Person>().HasCheckConstraint("CHK_TIN", "len([TaxIndentificationNumber]) = 8");

		}
		public List<Person> sp_GetAllPersons()
		{
			//Цей метод вказує EF Core використовувати чистий SQL-запит для отримання даних з бази даних. 
			return Persons.FromSqlRaw("EXECUTE [dbo].[GetAllPersons_Second]").ToList(); 
		}


		public int sp_InsertPerson(Person person)
		{
			var country = "USA";
			var countryId = Guid.NewGuid();
			SqlParameter[] parameters = new SqlParameter[] {
		new SqlParameter("@PersonId", person.PersonId),
		new SqlParameter("@Name", person.Name),
		new SqlParameter("@Email", person.Email),
		new SqlParameter("@DataOfBirth", person.DataOfBirth),
		new SqlParameter("@Gender", person.Gender),
		new SqlParameter("@Country", country),
		new SqlParameter("@CountryId",countryId),
		new SqlParameter("@Address", person.Address),
		new SqlParameter("@ReceiveNewsLetters", person.ReceiveNewsLetters)
	  };

			return Database.ExecuteSqlRaw("EXECUTE [dbo].[InsertPerson] @PersonId, @Name, @Email, @DataOfBirth, @Gender, @Country, @CountryId, @Address, @ReceiveNewsLetters", parameters);
		}

		
	}
}
