using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml.Linq;
using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
using Xunit.Abstractions;
using Xunit.Sdk;
using System.Threading.Tasks;

namespace CURDTests
{
	public class PersonServiceTest
	{
		private readonly IPersonService _person;
		private readonly IContriesService _contriesService;
		private readonly ITestOutputHelper _testOutput;
		public PersonServiceTest(ITestOutputHelper testOutputHelper)
		{
			_testOutput = testOutputHelper;
			_person = new PersonService(new DbContextPersons(new DbContextOptionsBuilder<DbContextPersons>().Options));
			_contriesService = new CountrirSersice(false);

		}

		#region AddPerson
		[Fact]
		public async Task AddPerson_NullPerson()
		{
			//Arrange
			PersonAddRequest? personAddRequest = null;

			//Act
			await Assert.ThrowsAsync<ArgumentNullException>(async() =>
			{
				await _person.AddPerson(personAddRequest);
			});
		}


		//When we supply null value as PersonName, it should throw ArgumentException
		[Fact]
		public async Task  AddPerson_PersonNameIsNull()
		{
			//Arrange
			PersonAddRequest? personAddRequest = new PersonAddRequest() { Name = null };

			//Act
			await Assert.ThrowsAsync<ArgumentException>( async() =>
			{
				await _person.AddPerson(personAddRequest);
			});
		}

		//When we supply proper person details, it should insert the person into the persons list; and it should return an object of PersonResponse, which includes with the newly generated person id
		[Fact]
		public async Task AddPerson_ProperPersonDetails()
		{
			//Arrange
			PersonAddRequest? personAddRequest = new PersonAddRequest() { Name = "Person name...", Email = "person@example.com", Address = "sample address", CountryId = Guid.NewGuid(), Gender = ServiceContracts.Enums.GenderOption.Male, DataOfBirth = DateTime.Parse("2000-01-01"), ReceiveNewsLetters = true };

			//Act
			PersonResponse person_response_from_add = await _person.AddPerson(personAddRequest);

			List<PersonResponse> persons_list = await _person.GetAllPersons();

			//Assert
			Assert.True(person_response_from_add.PersonId != Guid.Empty);

			Assert.Contains(person_response_from_add, persons_list);
		}




		#endregion



		#region GetByID

		[Fact]
		public async Task GetByPersonID_NullPersonID()
		{
			Guid? guid = null;
			PersonResponse? personResponse = await _person.GetPersonByID(guid);



			Assert.Null(personResponse);
		}

		[Fact]
		public async Task GetByPersonID_WhitPersonID()
		{

			CountryAddRequest countryAddRequest = new CountryAddRequest()
			{ CountryName = "Canada" };


			ContryResponse contryResponse = _contriesService.AddCountry(countryAddRequest);
			PersonAddRequest personAddRequest = new PersonAddRequest()
			{
				Name = "Oleg",
				Email = "email@same.com",
				Address = "address",
				CountryId = contryResponse.CountryID,
				DataOfBirth = DateTime.Parse("1900-10-10"),
				Gender = GenderOption.Male,
				ReceiveNewsLetters = false,
			};

			PersonResponse? personResponse = await _person.AddPerson(personAddRequest);

			PersonResponse? personResponse_from_get = await _person.GetPersonByID(personResponse.PersonId);

			Assert.Equal(personResponse_from_get, personResponse);



		}
		#endregion

		#region TEST
		[Fact]
		public async Task TestOutputHelper()
		{
			_testOutput.WriteLine("Test:");
			PersonResponse personResponse_1 = new PersonResponse()
			{
				Name = "Oleg",
				Address = "Lucasha",
				Age = 18
			};
			PersonResponse personResponse_2 = new PersonResponse()
			{
				Name = "Orest",
				Address = "Lucasha",
				Age = 18
			};
			List<PersonResponse> personResponseAll = new List<PersonResponse>();
			personResponseAll.Add(personResponse_1);
			personResponseAll.Add(personResponse_2);
			foreach (PersonResponse personResponseItem in personResponseAll)
			{
				_testOutput.WriteLine(personResponseItem.ToString());
			}

			Assert.Equal(1, 1);

		}
		#endregion




		#region GetAllPersons
		[Fact]
		public async Task GetAllPersons_EmptyLIst()
		{

			List<PersonResponse> personResponses_for_pass_test = await _person.GetAllPersons();

			Assert.Empty(personResponses_for_pass_test);
		}


		[Fact]
		public async Task GetAllPersons_AddFewPersons()
		{
			//Arrange
			CountryAddRequest country_request_1 = new CountryAddRequest() { CountryName = "USA" };
			CountryAddRequest country_request_2 = new CountryAddRequest() { CountryName = "India" };

			ContryResponse country_response_1 = _contriesService.AddCountry(country_request_1);
			ContryResponse country_response_2 = _contriesService.AddCountry(country_request_2);

			PersonAddRequest person_request_1 = new PersonAddRequest() { Name = "Smith", Email = "smith@example.com", Gender = GenderOption.Male, Address = "address of smith", CountryId = country_response_1.CountryID, DataOfBirth = DateTime.Parse("2002-05-06"), ReceiveNewsLetters = true };

			PersonAddRequest person_request_2 = new PersonAddRequest() { Name = "Mary", Email = "mary@example.com", Gender = GenderOption.Female, Address = "address of mary", CountryId = country_response_2.CountryID, DataOfBirth = DateTime.Parse("2000-02-02"), ReceiveNewsLetters = false };

			PersonAddRequest person_request_3 = new PersonAddRequest() { Name = "Rahman", Email = "rahman@example.com", Gender = GenderOption.Male, Address = "address of rahman", CountryId = country_response_2.CountryID, DataOfBirth = DateTime.Parse("1999-03-03"), ReceiveNewsLetters = true };

			List<PersonAddRequest> person_requests = new List<PersonAddRequest>() { person_request_1, person_request_2, person_request_3 };

			List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

			foreach (PersonAddRequest person_request in person_requests)
			{
				PersonResponse person_response = await _person.AddPerson(person_request);
				person_response_list_from_add.Add(person_response);
				_testOutput.WriteLine(person_response.ToString());
			}

			//Act
			List<PersonResponse> persons_list_from_get = await _person.GetAllPersons();

			//Assert
			foreach (PersonResponse person_response_from_add in person_response_list_from_add)
			{

				Assert.Contains(person_response_from_add, persons_list_from_get);
			}
		}

		#endregion

		#region GetFilteredPersons

		//If the search text is empty and search by is "PersonName", it should return all persons
		[Fact]
		public async Task GetFilteredPersons_EmptySearchText()
		{
			//Arrange
			CountryAddRequest country_request_1 = new CountryAddRequest() { CountryName = "USA" };
			CountryAddRequest country_request_2 = new CountryAddRequest() { CountryName = "India" };

			ContryResponse country_response_1 = _contriesService.AddCountry(country_request_1);
			ContryResponse country_response_2 = _contriesService.AddCountry(country_request_2);

			PersonAddRequest person_request_1 = new PersonAddRequest() { Name = "Smith", Email = "smith@example.com", Gender = GenderOption.Male, Address = "address of smith", CountryId = country_response_1.CountryID, DataOfBirth = DateTime.Parse("2002-05-06"), ReceiveNewsLetters = true };

			PersonAddRequest person_request_2 = new PersonAddRequest() { Name = "Mary", Email = "mary@example.com", Gender = GenderOption.Female, Address = "address of mary", CountryId = country_response_2.CountryID, DataOfBirth = DateTime.Parse("2000-02-02"), ReceiveNewsLetters = false };

			PersonAddRequest person_request_3 = new PersonAddRequest() { Name = "Rahman", Email = "rahman@example.com", Gender = GenderOption.Male, Address = "address of rahman", CountryId = country_response_2.CountryID, DataOfBirth = DateTime.Parse("1999-03-03"), ReceiveNewsLetters = true };

			List<PersonAddRequest> person_requests = new List<PersonAddRequest>() { person_request_1, person_request_2, person_request_3 };

			List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

			foreach (PersonAddRequest person_request in person_requests)
			{
				PersonResponse person_response = await _person.AddPerson(person_request);
				person_response_list_from_add.Add(person_response);
			}

			//print person_response_list_from_add
			_testOutput.WriteLine("Expected:");
			foreach (PersonResponse person_response_from_add in person_response_list_from_add)
			{
				_testOutput.WriteLine(person_response_from_add.ToString());
			}

			//Act
			List<PersonResponse> persons_list_from_search = await _person.GetFiltersPerson(nameof(Person.Name), "");

			//print persons_list_from_get
			_testOutput.WriteLine("Actual:");
			foreach (PersonResponse person_response_from_get in persons_list_from_search)
			{
				_testOutput.WriteLine(person_response_from_get.ToString());
			}

			//Assert
			foreach (PersonResponse person_response_from_add in person_response_list_from_add)
			{
				Assert.Contains(person_response_from_add, persons_list_from_search);
			}
		}


		//First we will add few persons; and then we will search based on person name with some search string. It should return the matching persons
		[Fact]
		public async Task GetFilteredPersons_SearchByPersonName()
		{
			//Arrange
			CountryAddRequest country_request_1 = new CountryAddRequest() { CountryName = "USA" };
			CountryAddRequest country_request_2 = new CountryAddRequest() { CountryName = "India" };

			ContryResponse country_response_1 = _contriesService.AddCountry(country_request_1);
			ContryResponse country_response_2 = _contriesService.AddCountry(country_request_2);

			PersonAddRequest person_request_1 = new PersonAddRequest() { Name = "Smith", Email = "smith@example.com", Gender = GenderOption.Male, Address = "address of smith", CountryId = country_response_1.CountryID, DataOfBirth = DateTime.Parse("2002-05-06"), ReceiveNewsLetters = true };

			PersonAddRequest person_request_2 = new PersonAddRequest() { Name = "Mary", Email = "mary@example.com", Gender = GenderOption.Female, Address = "address of mary", CountryId = country_response_2.CountryID, DataOfBirth = DateTime.Parse("2000-02-02"), ReceiveNewsLetters = false };

			PersonAddRequest person_request_3 = new PersonAddRequest() { Name = "Rahman", Email = "rahman@example.com", Gender = GenderOption.Male, Address = "address of rahman", CountryId = country_response_2.CountryID, DataOfBirth = DateTime.Parse("1999-03-03"), ReceiveNewsLetters = true };

			List<PersonAddRequest> person_requests = new List<PersonAddRequest>() { person_request_1, person_request_2, person_request_3 };

			List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

			foreach (PersonAddRequest person_request in person_requests)
			{
				PersonResponse person_response = await _person.AddPerson(person_request);
				person_response_list_from_add.Add(person_response);
			}

			//print person_response_list_from_add
			_testOutput.WriteLine("Expected:");
			foreach (PersonResponse person_response_from_add in person_response_list_from_add)
			{
				_testOutput.WriteLine(person_response_from_add.ToString());
			}

			//Act
			List<PersonResponse> persons_list_from_search = await _person.GetFiltersPerson(nameof(Person.Name), "ma");

			//print persons_list_from_get
			_testOutput.WriteLine("Actual:");
			foreach (PersonResponse person_response_from_get in persons_list_from_search)
			{
				_testOutput.WriteLine(person_response_from_get.ToString());
			}

			//Assert
			foreach (PersonResponse person_response_from_add in person_response_list_from_add)
			{
				if (person_response_from_add.Name != null)
				{
					if (person_response_from_add.Name.Contains("ma", StringComparison.OrdinalIgnoreCase))
					{
						Assert.Contains(person_response_from_add, persons_list_from_search);
					}
				}
			}
		}


		#endregion


		#region SortPerson

		[Fact]
		public async Task GetSortedPersons()
		{
			//Arrange
			CountryAddRequest country_request_1 = new CountryAddRequest() { CountryName = "USA" };
			CountryAddRequest country_request_2 = new CountryAddRequest() { CountryName = "India" };

			ContryResponse country_response_1 = _contriesService.AddCountry(country_request_1);
			ContryResponse country_response_2 = _contriesService.AddCountry(country_request_2);

			PersonAddRequest person_request_1 = new PersonAddRequest() { Name = "Smith", Email = "smith@example.com", Gender = GenderOption.Male, Address = "address of smith", CountryId = country_response_1.CountryID, DataOfBirth = DateTime.Parse("2002-05-06"), ReceiveNewsLetters = true };

			PersonAddRequest person_request_2 = new PersonAddRequest() { Name = "Mary", Email = "mary@example.com", Gender = GenderOption.Female, Address = "address of mary", CountryId = country_response_2.CountryID, DataOfBirth = DateTime.Parse("2000-02-02"), ReceiveNewsLetters = false };

			PersonAddRequest person_request_3 = new PersonAddRequest() { Name = "Rahman", Email = "rahman@example.com", Gender = GenderOption.Male, Address = "address of rahman", CountryId = country_response_2.CountryID, DataOfBirth = DateTime.Parse("1999-03-03"), ReceiveNewsLetters = true };

			List<PersonAddRequest> person_requests = new List<PersonAddRequest>() { person_request_1, person_request_2, person_request_3 };

			List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

			foreach (PersonAddRequest person_request in person_requests)
			{
				PersonResponse person_response = await _person.AddPerson(person_request);
				person_response_list_from_add.Add(person_response);
			}

			//print person_response_list_from_add
			_testOutput.WriteLine("Expected:");
			foreach (PersonResponse person_response_from_add in person_response_list_from_add)
			{
				_testOutput.WriteLine(person_response_from_add.ToString());
			}
			List<PersonResponse> allPersons = await _person.GetAllPersons();

			//Act
			List<PersonResponse> persons_list_from_sort = await _person.GetSortedPersons(allPersons, nameof(Person.Name), SortOrderOptions.DESC);

			//print persons_list_from_get
			_testOutput.WriteLine("Actual:");
			foreach (PersonResponse person_response_from_get in persons_list_from_sort)
			{
				_testOutput.WriteLine(person_response_from_get.ToString());
			}
			person_response_list_from_add = person_response_list_from_add.OrderByDescending(temp => temp.Name).ToList();

			//Assert
			for (int i = 0; i < person_response_list_from_add.Count; i++)
			{
				Assert.Equal(person_response_list_from_add[i], persons_list_from_sort[i]);
			}
		}

		#endregion


		#region UpDatePerson
		[Fact]
		public async Task UpdatePerson_NullPerson()
		{
			//Arrange
			PersonUpDateRequest? person_update_request = null;

			//Assert
			await Assert.ThrowsAsync<ArgumentNullException>(async () =>
			{
				//Act
				await _person.UpDatePerson(person_update_request);
			});
		}


		//When we supply invalid person id, it should throw ArgumentException
		[Fact]
		public async Task UpdatePerson_InvalidPersonID()
		{
			//Arrange
			PersonUpDateRequest? person_update_request = new PersonUpDateRequest() { PersonID = Guid.NewGuid() };

			//Assert
			await Assert.ThrowsAsync<ArgumentException>(async() =>
			{
				//Act
				await _person.UpDatePerson(person_update_request);
			});
		}


		//When PersonName is null, it should throw ArgumentException
		[Fact]
		public async Task UpdatePerson_PersonNameIsNull()
		{
			//Arrange
			CountryAddRequest country_add_request = new CountryAddRequest() { CountryName = "UK" };
			ContryResponse country_response_from_add = _contriesService.AddCountry(country_add_request);

			PersonAddRequest person_add_request = new PersonAddRequest()
			{
				Name = "John",
				CountryId = country_response_from_add.CountryID,
				Email = "oleh@shm.com",
				Address = "ffd",
				Gender = GenderOption.Male
			};
			PersonResponse person_response_from_add = await _person.AddPerson(person_add_request);

			PersonUpDateRequest person_update_request = person_response_from_add.ToPersonUpDateRequest();
			person_update_request.Name = null;


			//Assert
			await Assert.ThrowsAsync<ArgumentException>(async() =>
			{
				//Act
				await _person.UpDatePerson(person_update_request);
			});

		}


		//First, add a new person and try to update the person name and email
		[Fact]
		public async Task UpdatePerson_PersonFullDetailsUpdation()
		{
			//Arrange
			CountryAddRequest country_add_request = new CountryAddRequest() { CountryName = "UK" };
			ContryResponse country_response_from_add = _contriesService.AddCountry(country_add_request);

			PersonAddRequest person_add_request = new PersonAddRequest() { Name = "John", CountryId = country_response_from_add.CountryID, Address = "Abc road", DataOfBirth = DateTime.Parse("2000-01-01"), Email = "abc@example.com", Gender = GenderOption.Male, ReceiveNewsLetters = true };

			PersonResponse person_response_from_add = await _person.AddPerson(person_add_request);

			PersonUpDateRequest person_update_request = person_response_from_add.ToPersonUpDateRequest();
			person_update_request.Name = "William";
			person_update_request.Email = "william@example.com";

			//Act
			PersonResponse person_response_from_update = await _person.UpDatePerson(person_update_request);

			PersonResponse? person_response_from_get = await _person.GetPersonByID(person_response_from_update.PersonId);

			//Assert
			Assert.Equal(person_response_from_get, person_response_from_update);

		}

		#endregion


		#region Delete Person
		[Fact]
		public async Task DaletePerson_InvalidPersonID()
		{
			CountryAddRequest country_Add_Request = new CountryAddRequest()
			{
				CountryName = "USA"
			};
			ContryResponse contry_Response_from_add = _contriesService.AddCountry(country_Add_Request);

			PersonAddRequest personAddRequest = new PersonAddRequest()
			{
				Name = "Oleg",
				Address = "address",
				CountryId = contry_Response_from_add.CountryID,
				DataOfBirth = DateTime.Parse("2000-01-01"),
				Email = "abc@example.com",
				Gender = GenderOption.Male,
				ReceiveNewsLetters = true
			};
			PersonResponse person_Response_from_add = await _person.AddPerson(personAddRequest);

			bool isDelete = await _person.DeletePerson(person_Response_from_add.PersonId);
			Assert.True(isDelete);
		}

		[Fact]
		public async Task DaletePerson_InvalidPersonID_False()
		{


			bool isDelete = await _person.DeletePerson(Guid.NewGuid());
			Assert.False(isDelete);
		}
		#endregion
	}
}
