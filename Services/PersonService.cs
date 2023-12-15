using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using System.ComponentModel.DataAnnotations;
using Services.Helper;
using ServiceContracts.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using CsvHelper;
using System.IO;
using System.Globalization;

namespace Services
{
    
    public class PersonService : IPersonService
    {
        private readonly DbContextPersons _db;
		private readonly List<Person> Peoples;
        private readonly IContriesService contriesService;

		public CultureInfo CulterInfo { get; private set; }

		public PersonService(DbContextPersons dbContextPersons)
        {
            _db = dbContextPersons;                        
		}

        private PersonResponse ConvertPersonToPersonResponse(Person person)
        {
            PersonResponse personResponse = person.ToPersonResponse();
            //personResponse.Country = contriesService.GetCountryById(person.CountryId)?.CountryName;

            return personResponse;
        }

        public async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest)
        {
            if(personAddRequest == null)
            {
                throw new ArgumentNullException(nameof(personAddRequest));

            }
            else if(string.IsNullOrEmpty(personAddRequest.Name))
            {
                throw new ArgumentException("Name can't be blank");
            }

            ValidationHelper.ModelValidetion(personAddRequest);
            


            Person person =  personAddRequest.ToPerson();
            person.PersonId = Guid.NewGuid();



            _db.Persons.Add(person);
            await _db.SaveChangesAsync();

            //_db.sp_InsertPerson(person);

            return ConvertPersonToPersonResponse(person);
            

        }

        public async Task<List<PersonResponse>> GetAllPersons()
        {
			return _db.Persons.Select(temp => temp.ToPersonResponse()).ToList();
			//return _db.sp_GetAllPersons().Select(temp => temp.ToPersonResponse()).ToList();
		}

        public async Task<PersonResponse?> GetPersonByID(Guid? personID)
        {
            if (personID == null)
            {
                return null;
            }
            Person? person = await _db.Persons.FirstOrDefaultAsync(temp => temp.PersonId == personID);
            if (person == null)
                return null;
            return ConvertPersonToPersonResponse(person);
        }

        public async Task<List<PersonResponse>> GetFiltersPerson(string SeachBy, string SearchString)
        {
            List<PersonResponse> allPersons = await GetAllPersons();
            List<PersonResponse> machingPerson = allPersons;
            if (SeachBy == null || SearchString == null)
            {
                
                return machingPerson;
            }
            switch (SeachBy)
            {
                case nameof(PersonResponse.Name):
                    machingPerson = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.Name) ? temp.Name.Contains(SearchString,StringComparison.OrdinalIgnoreCase):true)).ToList();
                    break;

                case nameof(PersonResponse.Address):
                    machingPerson = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.Address) ? temp.Address.Contains(SearchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

				case nameof(PersonResponse.DataOfBirth):
					machingPerson = allPersons.Where(temp => ((temp.DataOfBirth!=null) ? temp.DataOfBirth.Value.ToString("dd MMMM yyyy").Contains(SearchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
					break;


				case nameof(PersonResponse.Gender):
					machingPerson = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.Gender) ? temp.Gender.Contains(SearchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
					break;

				case nameof(PersonResponse.CountryId):
					machingPerson = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.Country) ? temp.Country.Contains(SearchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
					break;

				case nameof(PersonResponse.Email):
                    machingPerson = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.Email) ? temp.Email.Contains(SearchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                default: machingPerson = allPersons;
                    break;
            }
            return machingPerson;



        }

		
		public async Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder)
		{
			if (string.IsNullOrEmpty(sortBy))
				return allPersons;

			List<PersonResponse> sortedPersons = (sortBy, sortOrder) switch
			{
				(nameof(PersonResponse.Name), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Name, StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.Name), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Name, StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.Email), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.Email), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.DataOfBirth), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.DataOfBirth).ToList(),

				(nameof(PersonResponse.DataOfBirth), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.DataOfBirth).ToList(),

				(nameof(PersonResponse.Age), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Age).ToList(),

				(nameof(PersonResponse.Age), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Age).ToList(),

				(nameof(PersonResponse.Gender), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.Gender), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.Country), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.Country), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.Address), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.Address), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.ReceiveNewsLetters).ToList(),

				(nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.ReceiveNewsLetters).ToList(),

				_ => allPersons
			};

			return sortedPersons;
		}

		public async Task<PersonResponse> UpDatePerson(PersonUpDateRequest? personUpdateRequest)
		{
            if (personUpdateRequest != null)
            {
                ValidationHelper.ModelValidetion(personUpdateRequest);

                Person? matchingPerson = await _db.Persons.FirstOrDefaultAsync(temp => temp.PersonId == personUpdateRequest.PersonID);
                if(matchingPerson == null)
                {
                    throw new ArgumentException("This field should be fiiled");
                }

				matchingPerson.Name = personUpdateRequest.Name;
				matchingPerson.Email = personUpdateRequest.Email;
				matchingPerson.DataOfBirth = personUpdateRequest.DataOfBirth;
				matchingPerson.Gender = personUpdateRequest.Gender.ToString();
				matchingPerson.CountryId = personUpdateRequest.CountryId;
				matchingPerson.Address = personUpdateRequest.Address;
				matchingPerson.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;

                await _db.SaveChangesAsync();

                return ConvertPersonToPersonResponse(matchingPerson);
			}
            else
            {
                throw new ArgumentNullException(nameof(Person));
            }
		}

		public async Task<bool> DeletePerson(Guid? personID)
		{
			if (personID != null)
			{
                Person? personId = await _db.Persons.FirstOrDefaultAsync(temp => temp.PersonId == personID);
                if (personId == null)
                {
                    return false;
                }
                _db.Persons.Remove(_db.Persons.First( temp => temp.PersonId == personID));
                await _db.SaveChangesAsync();
                return true;
			}
			else
			{
				throw new NotImplementedException(nameof(personID));

			}
			
		}

		//public async Task<MemoryStream> GetPersonCSV()
		//{
  //          MemoryStream memoryStream = new MemoryStream();

		//	// Створюється об'єкт StreamWriter, який дозволяє записувати дані в memoryStream
		//	StreamWriter streamWriter = new StreamWriter(memoryStream);

		//	//Він приймає streamWriter для запису в memoryStream, а також об'єкт CultureInfo для визначення формату чисел та дат.
		//	CsvWriter csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture, leaveOpen: true);

		//	// Це означає, що першим рядком у файлі будуть назви полів цього класу.
		//	csvWriter.WriteHeader<PersonResponse>();

		//	//Переходить до наступного рядка у файлі CSV.
		//	csvWriter.NextRecord();

  //          List<PersonResponse> personResponses = _db.Persons.Include("Person.cs").Select(temp => temp.ToPersonResponse()).ToList();

		//	//Асинхронно записує дані зі списку personResponses у файл CSV.
		//	await csvWriter.WriteRecordsAsync(personResponses); 

  //          memoryStream.Position = 0;
  //          return memoryStream;
		//}
	}
}

