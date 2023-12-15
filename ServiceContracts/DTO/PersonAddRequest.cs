using System;
using System.Collections.Generic;
using ServiceContracts.Enums;
using Entities;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO
{
    public class PersonAddRequest
    {

        [Required(ErrorMessage ="Person name must be blank")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Email name must be blank")]
        [EmailAddress(ErrorMessage = "Types should be email")]
		[DataType(DataType.EmailAddress)]
		public string? Email { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Date Of Birth name must be blank")]
        public DateTime? DataOfBirth { get; set; }

		[Required(ErrorMessage = "Gender name must be blank")]
		public GenderOption? Gender { get; set; }
        public Guid? CountryId { get; set; }

		[Required(ErrorMessage = "Address name must be blank")]
		public string? Address { get; set; }

		[Required(ErrorMessage = "Receive News Letters name must be blank")]
		public bool ReceiveNewsLetters { get; set; }

        public Person ToPerson()
        {
            return new Person()
            {
                Name = Name,
                Email = Email,
                DataOfBirth = DataOfBirth,
                Gender = Gender.ToString(),
                CountryId = CountryId,
                Address = Address,
                ReceiveNewsLetters = ReceiveNewsLetters
            };
        }
    }
}
