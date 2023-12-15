using Entities;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO
{
	public class PersonUpDateRequest
	{
		
			[Required(ErrorMessage ="Person ID can't be blank")]
			public Guid PersonID { get; set;}

			[Required(ErrorMessage = "Person name must be blank")]
			public string? Name { get; set; }

			[Required(ErrorMessage = "Email name must be blank")]
			[EmailAddress(ErrorMessage = "Types should be email")]
			public string? Email { get; set; }

			public DateTime? DataOfBirth { get; set; }
			public GenderOption? Gender { get; set; }
			public Guid? CountryId { get; set; }
			public string? Address { get; set; }
			public bool? ReceiveNewsLetters { get; set; }

			public Person ToPerson()
			{
				return new Person()
				{
					PersonId = PersonID,
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

