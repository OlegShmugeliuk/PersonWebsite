using System;
using System.Collections.Generic;
using Entities;
using Xunit;
using ServiceContracts;
using Services;
using ServiceContracts.DTO;


namespace CURDTests
{
    public class CountriesServiceTest
    { 
        readonly IContriesService _contriesService;
		public CountriesServiceTest()
		{
			_contriesService = new CountrirSersice(false);
		}

		#region GatAllCountries
		[Fact]
        public void GetAllContries_EmptyList()
        {
            List<ContryResponse> countries_response_list = _contriesService.GetAllCountries();

            Assert.Empty(countries_response_list);
        }

        [Fact]
        public void GetAllContries_AddFewCountries()
        {
            List<CountryAddRequest> countryAddRequests = new List<CountryAddRequest>()
            {
                new CountryAddRequest() {CountryName = "USA" },
                new CountryAddRequest() {CountryName = "US" },
                new CountryAddRequest() {CountryName = "UK" },
            };

            List<ContryResponse> contryResponses = new List<ContryResponse>();
            foreach (var item in countryAddRequests)
            {
                contryResponses.Add(_contriesService.AddCountry(item));
            }

            List<ContryResponse> actualCountry = _contriesService.GetAllCountries();
            foreach (ContryResponse exepted in contryResponses) {
                Assert.Contains(exepted, actualCountry);
            }


        }

        #endregion

        

        #region AddCountry



        [Fact]
        //Коли CountryAddRequest == null має викуниту виняток ArgumentNullException
        public void AddCoutry_Null_Country()
        {
            //Arrange(Упорядкувати)
            CountryAddRequest? request = null;





            //Assert(підтвердження)
            Assert.Throws<ArgumentNullException>(() => {
                //Act(дія)
                _contriesService.AddCountry(request);
            });

        }


        //Коли CountryName == null має викуниту виняток ArgumentException
        [Fact]
        public void AddCoutry_CountryName_Null()
        {
            //Arrange(Упорядкувати)
            CountryAddRequest? request = new CountryAddRequest() {
                CountryName = null
            };

            //Assert(підтвердження)
            Assert.Throws<ArgumentException>(() =>
            {
                //Act(дія)
                _contriesService.AddCountry(request);
            });

        }

        [Fact]
        //Коли CountryName поторюється знову  має викуниту виняток ArgumentException
        public void AddCoutry_Dublicate_CountryName()
        {
            //Arrange(Упорядкувати)
            CountryAddRequest? request_1 = new CountryAddRequest()
            {
                CountryName = "UK"
            };
            CountryAddRequest? request_2 = new CountryAddRequest()
            {
                CountryName = "UK"
            };

            //Assert(підтвердження)
            Assert.Throws<ArgumentException>(() =>
            {
                //Act(дія)
                _contriesService.AddCountry(request_1);
                _contriesService.AddCountry(request_2);
            });

        }



        //коли ви вказуєте правильну назву країни має додати її у визначений список країн

        [Fact]
        public void AddCoutry_ProperCountryDetails()
        {
            //Arrange(Упорядкувати)
            CountryAddRequest? request_1 = new CountryAddRequest()
            {
                CountryName = "Japan"
            };

            //Act(дія)
            ContryResponse response = _contriesService.AddCountry(request_1);
            List<ContryResponse> contryResponses = _contriesService.GetAllCountries();





            //Assert(підтвердження)
            Assert.True(response.CountryID != Guid.Empty);
            Assert.Contains(response, contryResponses);

        }
        #endregion


        #region GetCountryByID

        [Fact]
        public void GetCountryByCountryID_NullCountryID()
        {
            Guid? countryId = null;

            ContryResponse country_response_from_get_method =  _contriesService.GetCountryById(countryId);

            Assert.Null(country_response_from_get_method);
        }

        [Fact]
        public void GetCountryByCountryID_ValidCountryID()
        {
            CountryAddRequest? countryAddRequest = new CountryAddRequest()
            {
                CountryName = "Usa",
            };
            ContryResponse country_response_from_add = _contriesService.AddCountry(countryAddRequest);

            ContryResponse? country_response_from_get = _contriesService.GetCountryById(country_response_from_add.CountryID);

            Assert.Equal(country_response_from_add, country_response_from_get);

        }
        #endregion
    }
}
