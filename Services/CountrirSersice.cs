using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountrirSersice : IContriesService
    {

        private readonly List<Country> _countries;
        public CountrirSersice(bool initialize = true)
        {
            _countries = new List<Country>();
            if (initialize)
            {
                _countries.AddRange(new List<Country>() {
                new Country()
                {
                    CountryID = Guid.Parse("AC1CEEAC-92A9-4709-9AF2-D662A7BA6C41"),
                    CountryName = "USA"
                },
                new Country()
                {
                    CountryID = Guid.Parse("E0C8AD2F-E1E1-48A8-A12F-DEF247F8DF64"),
                    CountryName = "Ukraine"
                },

                new Country()
                {
                    CountryID = Guid.Parse("10F23E37-CB1E-4811-8FB2-200200F68F6B"),
                    CountryName = "Kanada"
                },

                new Country()
                {
                    CountryID = Guid.Parse("ECB078B4-A249-4E7F-B304-E85F7FEEE310"),
                    CountryName = "UK"
                },

                new Country()
                {
                    CountryID = Guid.Parse("B1A25B78-1A76-48F4-A780-C605DA17CDC3"),
                    CountryName = "Poland"
                }
                });
			}
		}

        public ContryResponse AddCountry(CountryAddRequest? countryAddRequest)
        {
            if (countryAddRequest == null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest));
            }
            if (countryAddRequest.CountryName == null)
            {
                throw new ArgumentException(nameof(countryAddRequest.CountryName));
            }

            if (_countries.Where(x => x.CountryName == countryAddRequest.CountryName).Count() > 0) {
                throw new ArgumentException("This name already existe");
            }



            Country country = countryAddRequest.ToCountry();
            country.CountryID = Guid.NewGuid();

            _countries.Add(country);
            return country.ToCountryResponse();

        }

        public List<ContryResponse> GetAllCountries()
        {
            return _countries.Select(country => country.ToCountryResponse()).ToList();
        }

        public ContryResponse? GetCountryById(Guid? contryID)
        {
            if(contryID != null)
            {
                Country? contryResponse = _countries.FirstOrDefault(_countryId => _countryId.CountryID == contryID);

                if (contryResponse==null) {
                    return null;
                }
                return contryResponse.ToCountryResponse();
            }
            else
            {
                return null;
            }
        }
    }
}