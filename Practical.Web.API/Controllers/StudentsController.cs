using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Practical.Web.API.Models;

namespace Practical.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {

        private static readonly List<Student> student = new List<Student>
        {
            new Student { Id = 1, Name = "John Doe", Gender = "Male", City = "New York", Department = "HR" },
            new Student { Id = 2, Name = "Jane Smith", Gender = "Female", City = "Los Angeles", Department = "Finance" },
            new Student { Id = 3, Name = "Mike Johnson", Gender = "Male", City = "Chicago", Department = "IT" }
        };

        

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
            return new Student()
            {
                Id = 1001,
                Name = "Anurag",
                City = "Mumbai",
                Gender = "Male",
                Department = "IT"
            };
        }

        [Route("All")]
        [HttpGet]
        public List<Student> GetAllStudents()
        {
            return new List<Student> {

                new Student { Id = 1001, Name = "Anurag", City = "Mumbai", Gender = "Male", Department = "IT" },
                new Student { Id = 1002, Name = "Pranaya", City = "Delhi", Gender = "Male", Department = "IT" },
                new Student { Id = 1003, Name = "Priyanka", City = "BBSR", Gender = "Female", Department = "HR" }
            };
        }

        [Route("GetStudents")]
        [HttpGet]
        public ActionResult<Student> GetStudent()
        {

            var students = new List<Student> {

                new Student { Id = 1001, Name = "Anurag", City = "Mumbai", Gender = "Male", Department = "IT" },
                new Student { Id = 1002, Name = "Pranaya", City = "Delhi", Gender = "Male", Department = "IT" },
                new Student { Id = 1003, Name = "Priyanka", City = "BBSR", Gender = "Female", Department = "HR" }
            };

            try
            {
                //If at least one Employee is Present return OK status code and the list of employees
                if (students.Any())
                {
                    return Ok(students);
                }
                else
                {
                    //If no Employee is Present return Not Found Status Code
                    return NotFound("No student match the provided search criteria.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request");
            };
        }

        [Route("{id:int}")]
        [HttpGet]

        //As we are going to return Ok, StatusCode, and NotFound Result from this action method,
        //we are using IActionResult as the return type of this method
        //200 OK: Returns a list of Employee objects.
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Student>))]

        //404 Not Found: Indicates that no employees were found.
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        //500 Internal Server Error: Indicates an unexpected error during processing.
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public IActionResult GetStudentDetailById(int id)
        {

            try
            {

                var listStudents = new List<Student>
            {
                new Student { Id = 1001, Name = "Anurag", City = "Mumbai", Gender = "Male", Department = "IT" },
                new Student { Id = 1002, Name = "Pranaya", City = "Delhi", Gender = "Male", Department = "IT" },
                new Student { Id = 1003, Name = "Priyanka", City = "BBSR", Gender = "Female", Department = "HR" }
            };

                //Fetch the Employee details by the provided ID
                var student = listStudents.FirstOrDefault(std => std.Id == id);

                if (student != null)
                {
                    return Ok(student);
                }
                else
                {
                    //If Employee Does Not Exists Return NotFound
                    return NotFound("Student does not exist.");
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
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
