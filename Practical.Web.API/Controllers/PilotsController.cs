using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Practical.Web.API.Models;

namespace Practical.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PilotsController : ControllerBase
    {
        private static List<PilotModel> pilot = new List<PilotModel> {

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
                filtedPilots = pilot.Where(
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
    }
}
