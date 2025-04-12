using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Practical.Web.API.Controllers
{
    
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        [Route("api/[controller]/[action]")]
        [HttpGet]
        public string GetAllEmployees()
        {
            return "Response from GetAllEmployees Method";
        }

        [Route("api/[controller]/[action]/{Id}")]
        [HttpGet]
        public string GetEmployeeById(int Id)
        {
            return $"Reponse from GetEmployeeById Method Id: {Id}";
        }
    }
}
