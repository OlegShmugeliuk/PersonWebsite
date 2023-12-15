using ServiceContracts.DTO;

using System.ComponentModel;

namespace ServiceContracts
{
    public interface IContriesService
    {
        ContryResponse AddCountry(CountryAddRequest? countryAddRequest);

        List<ContryResponse> GetAllCountries();

        ContryResponse? GetCountryById(Guid? contryID);

    }
}