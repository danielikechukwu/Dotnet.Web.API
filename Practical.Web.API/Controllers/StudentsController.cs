using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Practical.Web.API.Models;

namespace Practical.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {

        [Route("{gender}/{city}")]
        [HttpGet]
        public ActionResult<Student> GetStudentByGenderAndCity(string gender, string city)
        {
            var filteredStudents = StudentData.Students.Where(e => e.Gender.Equals(gender, StringComparison.OrdinalIgnoreCase) &&
            e.City.Equals(city, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!filteredStudents.Any())
            {
                return NotFound($"No Employee found with Gender '{gender}' in City '{city}'");
            }

            return Ok(filteredStudents);

        }

        //Returning a primitive data type
        [Route("Name")]
        [HttpGet]
        public string GetName()
        {
            return "Return from GetName";
        }

        //Returning a complex type
        [Route("Details")]
        [HttpGet]
        public ActionResult<Student> GetDetails()
        {
            return new Student() {
                Id = 1001,
                Name = "Anurag",
                City = "Mumbai",
                Gender = "Male",
                Department = "IT"
            };
        }


        [Route("Search")]
        [HttpGet]
        public ActionResult<IEnumerable<Student>> SearchStudent([FromQuery] string department)
        {
            var filteredStudents = StudentData.Students.Where(e => e.Department.Equals(department, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!filteredStudents.Any())
            {
                return NotFound($"No employees found in Department '{department}'.");
            }
            return Ok(filteredStudents);
        }

        // GET api/Employee/Search?Gender=Male&Department=IT&City=Los Angeles
        [Route("Searchs")]
        [HttpGet]
        public ActionResult<IEnumerable<Student>> SearchStudents([FromQuery] StudentSearch searchCriteria)
        {
            var filteredStudents = StudentData.Students.AsQueryable();

            if (!string.IsNullOrEmpty(searchCriteria.Gender))
                filteredStudents = filteredStudents.Where(e => e.Gender.Equals(searchCriteria.Gender, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(searchCriteria.Department))
                filteredStudents = filteredStudents.Where(e => e.Department.Equals(searchCriteria.Department, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(searchCriteria.City))
                filteredStudents = filteredStudents.Where(e => e.City.Equals(searchCriteria.City, StringComparison.OrdinalIgnoreCase));

            var result = filteredStudents.ToList();

            if (!result.Any())
                return NotFound("No employees match the provided search criteria.");

            return Ok(result);
        }

        // GET api/Student/DirectSearch?Gender=Male&Department=IT
        [Route("DirectSearch")]
        [HttpGet]
        public ActionResult<IEnumerable<Employee>> DirectSearchStudents()
        {
            string gender = HttpContext.Request.Query["Gender"].ToString();

            string department = HttpContext.Request.Query["Department"].ToString();

            string city = HttpContext.Request.Query["City"].ToString();

            var filteredStudents = StudentData.Students.AsQueryable();

            if (!string.IsNullOrEmpty(gender))
                filteredStudents = filteredStudents.Where(e => e.Gender.Equals(gender, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(department))
                filteredStudents = filteredStudents.Where(e => e.Department.Equals(department, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(city))
                filteredStudents = filteredStudents.Where(e => e.City.Equals(city, StringComparison.OrdinalIgnoreCase));

            var result = filteredStudents.ToList();

            if (!result.Any())
                return NotFound("No employees match the provided search criteria.");

            return Ok(result);


        }

        [Route("{gender}")]
        [HttpGet]
        public ActionResult<IEnumerable<Student>> GetStudentsByGender([FromRoute] string gender, [FromQuery] string? department, [FromQuery] string? city)
        {
            var filteredStudents = StudentData.Students.Where(e => e.Gender.Equals(gender, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(department))
                filteredStudents = filteredStudents.Where(e => e.Department.Equals(department, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(city))
                filteredStudents = filteredStudents.Where(e => e.City.Equals(city, StringComparison.OrdinalIgnoreCase));

            var result = filteredStudents.ToList();

            if (!result.Any())
                return NotFound("No employees match the provided search criteria.");

            return Ok(result);
        }


    }
}
