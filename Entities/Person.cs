using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Person
    {
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Додайте цей атрибут для автоматичного генерації значень
		public Guid PersonId { get; set; }
		
		[StringLength(40)]// максимальна довжина стрічки
		public string? Name { get; set; }

		[StringLength(40)]
		public string? Email { get; set; }
        public DateTime? DataOfBirth { get; set; }
		[StringLength(10)]

		public string? Gender { get; set; }
        public string? Country { get; set; }
		
		public Guid? CountryId { get; set; }

		[StringLength(100)]
		public string? Address { get; set; }
		
		public bool? ReceiveNewsLetters { get; set; }

		//text indefication number
		public string? TIN {  get; set; }	
    }
}
