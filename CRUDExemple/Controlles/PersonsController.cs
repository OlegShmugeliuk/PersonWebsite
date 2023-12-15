using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using System.IO;

namespace CRUDExemple.Controlles
{
    [Route("[controller]")]
    public class PersonsController : Controller
    {
        private readonly IPersonService _personService;
        private readonly IContriesService _contriesService;
        public PersonsController(IPersonService personService, IContriesService contriesService)
        {
            _personService = personService;
            _contriesService = contriesService;
        }
		[Route("[action]")]
		[Route("/")]
		public async Task<IActionResult> Index(string searchBy, string? searchString, string SortBy = nameof(PersonResponse.Name), SortOrderOptions sortOrder = SortOrderOptions.ASC)
        {

            ViewBag.SearchFields = new Dictionary<string, string>() {
                { nameof(PersonResponse.Name),"Person Name" },
				{ nameof(PersonResponse.Email),"Email" },
				{ nameof(PersonResponse.DataOfBirth),"Date of Birth" },
				{ nameof(PersonResponse.Gender),"Gender" },
				{ nameof(PersonResponse.Country),"Country" },
				{ nameof(PersonResponse.Address),"Address" }
				
				//{ nameof(PersonResponse.Name),"Person Name" },
				//{ nameof(PersonResponse.Name),"Person Name" },
			};
            List<PersonResponse> persons = await _personService.GetFiltersPerson(searchBy, searchString);
            ViewBag.CurrentSearchBy = searchBy;
            ViewBag.CurrentSearchString = searchString;

            //Sort
            List<PersonResponse> sortedPersons = await _personService.GetSortedPersons(persons, SortBy, sortOrder);
            ViewBag.CurrentSotrBy = SortBy;
            ViewBag.CurrentSotrOrder = sortOrder.ToString();
            return View(sortedPersons);
        }

        [Route("create")]
        [HttpGet]
        public async Task<IActionResult> CreateView()
        {
            List<ContryResponse> countries = _contriesService.GetAllCountries();
            ViewBag.Countries = countries;

            //new SelectListItem() { Text = "Oleg", Value = "1" };
            //<option value = 1>Oleg</option>
            return View();
        }

        [HttpPost]
		[Route("create")]
		public async Task<IActionResult> CreateView(PersonAddRequest personAddRequest)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e=>e.ErrorMessage).ToList();
				return View();
			}
            PersonResponse personResponse = await _personService.AddPerson(personAddRequest);
            
            return RedirectToAction("Index", "Persons");
        }

        [HttpGet]
        [Route("[action]/{personID}")]
        public async Task<IActionResult> Edit(Guid personID)
        {
            PersonResponse? personResponse = await _personService.GetPersonByID(personID);
            if (personResponse == null)
            {
                return RedirectToAction("Index");
            }

            PersonUpDateRequest personUpDateRequest = personResponse.ToPersonUpDateRequest();
            return View(personUpDateRequest);
        }

        [HttpPost]
		[Route("[action]/{personID}")]
		public async Task<IActionResult> Edit(PersonUpDateRequest personUpDate)
        {
            PersonResponse? personResponse = await _personService.GetPersonByID(personUpDate.PersonID);
            if (personResponse == null)
            {
				return RedirectToAction("Index");
			}

            if (ModelState.IsValid)
            {
               PersonResponse personResponseUpData = await _personService.UpDatePerson(personUpDate);
                return RedirectToAction("Index");           
            }
            else
            {
				ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
				return View();
			}
            
        }

		[HttpGet]
		[Route("[action]/{personID}")]
		public async Task<IActionResult> Delete(Guid? personID)
        {
            Console.WriteLine(personID);
            PersonResponse personResponse =await _personService.GetPersonByID(personID);
            if (personResponse == null)
            {
				return RedirectToAction("Index");
			}

            return View(personResponse);
        }

        [HttpPost]		
		[Route("[action]/{personID}")]
		public async Task<IActionResult> Delete(PersonUpDateRequest personUpDate)
		{
            PersonResponse? personResponse = await _personService.GetPersonByID(personUpDate.PersonID);
            if (personResponse == null)
				return RedirectToAction("Index");

			await _personService.DeletePerson(personUpDate.PersonID);
			return RedirectToAction("Index");
		}


        [Route("CreatePDF")]
        public async Task<IActionResult> CreatePDF()
        {
            List<PersonResponse>persons = await _personService.GetAllPersons();
            return new ViewAsPdf("CreatePDF", persons, ViewData) {
                PageMargins = new Rotativa.AspNetCore.Options.Margins()
                {
                    Top = 20,
                    Right = 20,
                    Bottom = 20,
                    Left = 20
                },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };            
        }

		//[Route("PersonCSV")]
  //      public async Task<IActionResult> PersonCSV()
  //      {
  //          MemoryStream memoryStream = await _personService.GetPersonCSV();
  //          return File(memoryStream, "application/octet-stream", "person.csv");
  //      }
	}
}
