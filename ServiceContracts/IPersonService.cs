using System;
using System.Collections.Generic;
using System.Xml.XPath;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts
{
    public interface IPersonService
    {
        Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest);

        Task<List<PersonResponse>> GetAllPersons();

		Task<PersonResponse?> GetPersonByID(Guid? guid);

        Task<List<PersonResponse>> GetFiltersPerson(string SeachBy, string SearchEtring);
        

        public Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder);

        public Task<PersonResponse> UpDatePerson(PersonUpDateRequest? personUpDateRequest);

        public Task<bool> DeletePerson(Guid? personID);

        //public Task<MemoryStream> GetPersonCSV();
	}

    
}
