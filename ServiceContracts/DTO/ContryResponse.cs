using System;
using System.Collections.Generic;
using Entities;

namespace ServiceContracts.DTO
{
    public class ContryResponse
    {
        public Guid CountryID { get; set; }
        public string? CountryName { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj.GetType() != typeof(ContryResponse))
            {
                return false;

            }
            
            ContryResponse contryResponse = (ContryResponse)obj as ContryResponse;
            return CountryID == contryResponse.CountryID && CountryName == contryResponse.CountryName;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public static class CountryExtensions { 
        public static ContryResponse ToCountryResponse(this Country country)
        {
            return new ContryResponse()
            {
                CountryID = country.CountryID,
                CountryName = country.CountryName
            };
        }
    }
}
