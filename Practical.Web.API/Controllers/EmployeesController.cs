using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Practical.Web.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {

        [HttpGet]
        public string Get()
        {
            return "Returning from employee controller Get Method";
        }

        [HttpGet]
        public string GetEmployee()
        {
            return "Returning from employee controller GetEmployee Method";
        }
    }
}
