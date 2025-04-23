using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Practical.Web.API.Models;

namespace Practical.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PilotsController : ControllerBase
    {
        private static List<PilotModel> Pilot = new List<PilotModel> {

            new PilotModel { Id = 1, Name = "Rakesh", Department = "IT", Gender = "Male", Salary = 1000 },
            new PilotModel { Id = 2, Name = "Priyanka", Department = "IT", Gender = "Female", Salary = 2000  },
            new PilotModel { Id = 3, Name = "Suresh", Department = "HR", Gender = "Male", Salary = 3000 },
            new PilotModel { Id = 4, Name = "Hina", Department = "HR", Gender = "Female", Salary = 4000 },
            new PilotModel { Id = 5, Name = "Pranaya", Department = "HR", Gender = "Male", Salary = 35000 },
            new PilotModel { Id = 6, Name = "Pooja", Department = "IT", Gender = "Female", Salary = 2500 },
        };

        [HttpGet]
        public ActionResult<IList<PilotModel>> GetPilots([FromQuery] PilotSearch pilotSearch)
        {
            // Implementation to retrieve employees based on the Department
            var filtedPilots = new List<PilotModel>();

            if(pilotSearch != null)
            {
                filtedPilots = Pilot.Where(
                    p => p.Department.Equals(pilotSearch.Department, StringComparison.OrdinalIgnoreCase) || 
                    p.Gender.Equals(pilotSearch.Gender, StringComparison.OrdinalIgnoreCase)).ToList();

                if (filtedPilots.Count > 0)
                {
                    return Ok(filtedPilots);
                }
                return NotFound($"No Users Found with Department: {pilotSearch?.Department} and Gender: {pilotSearch?.Gender}");

            }

            return BadRequest("Invalid Search Criteria");
            
        }

        // Now, you can use "ProductId" to refer to the parameter that comes from the route.
        // Fetch the Product by the ProductId and return a response

        [HttpGet("{id}")]
        public ActionResult<PilotModel> GetPilotById([FromRoute(Name = "id")] int pilotId)
        {
            var pilot = Pilot.FirstOrDefault(p => p.Id == pilotId);

            if(pilot != null)
            {
                return Ok(pilot);
            }

            return NotFound($"No Product Found with Product Id: {pilotId}");
        }

        [HttpGet("{Name}/{Department}")]
        public ActionResult<PilotModel> GetPilotByNameCategory([FromRoute] PilotRoute pilotRoute)
        {
            if(pilotRoute != null)
            {
                var filteredPilot = Pilot.Where(p => p.Name.ToLower().StartsWith(pilotRoute.Name.ToLower()) &&
                p.Department.ToLower() == pilotRoute.Department.ToLower()).ToList();

                return Ok(filteredPilot);
            }

            return BadRequest("Invalid Search Criteria");
        }
    }
}
