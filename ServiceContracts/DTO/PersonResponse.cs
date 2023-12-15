using Entities;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ServiceContracts.DTO
{
    public class PersonResponse
    {
        public Guid PersonId { get; set; }
        public string? Name { get; set; }
        public double? Age { get; set; }
        public string? Email { get; set; }
        public DateTime? DataOfBirth { get; set; }
        public string? Gender { get; set; }
        public Guid? CountryId { get; set; }
        public string? Country { get; set; }
        public string? Address { get; set; }
        public bool? ReceiveNewsLetters { get; set; }

		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType())
			{
				return false;
			}

			PersonResponse other = (PersonResponse)obj;

			// Compare properties for equality
			return PersonId == other.PersonId && Name == other.Name && Email == other.Email
				&& Address == other.Address && CountryId == other.CountryId&&DataOfBirth == other.DataOfBirth &&
				ReceiveNewsLetters == other.ReceiveNewsLetters && Gender==other.Gender;


		}

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"Name: {Name}, Address: {Address}, Age: {Age}, Email: {Email}";
        }

        public PersonUpDateRequest ToPersonUpDateRequest()
        {
            return new PersonUpDateRequest()
            {
                PersonID = PersonId,
                Name = Name,
                Address = Address,
                CountryId = CountryId,
                Email = Email,
                DataOfBirth = DataOfBirth,
				Gender = (GenderOption)Enum.Parse(typeof(GenderOption), Gender, true),

				ReceiveNewsLetters = ReceiveNewsLetters
            };
        }
    }

    public static class PersonExtensions
    {
        public static PersonResponse ToPersonResponse(this Person person)
        {
            return new PersonResponse()
            {
                Name = person.Name,
                PersonId = person.PersonId,
                Email = person.Email,
                Address = person.Address,
                ReceiveNewsLetters = person.ReceiveNewsLetters,
                DataOfBirth = person.DataOfBirth,
                CountryId = person.CountryId,
                Gender = person.Gender,
                Age = (person.DataOfBirth != null) ? Math.Round((DateTime.Now - person.DataOfBirth.Value).TotalDays / 365.25) : null
                
            };
        }
    }
}
