using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Practical.Web.API.Models;
using Practical.Web.API.Repositories;

namespace Practical.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {

        public readonly IEmployeeRepository _repository;

        //Injects IEmployeeRepository to manage data operations.
        public EmployeesController(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        //Action method with multiple route        
        [HttpGet("GetAll")]
        [HttpGet("AllEmployees")]
        public ActionResult<IEnumerable<Employee>> GetAllEmployees()
        {
            var employees = _repository.GetAll();

            return Ok(employees);

        }

        //Action method with multiple route        
        [HttpGet("All")]
        public ActionResult<IEnumerable<Employee>> GetEmployees()
        {
            var employees = _repository.GetAll();

            return Ok(employees);

        }

        [Route("{EmployeeId:int}")]
        [HttpGet]
        public string GetEmployeeDetails(int EmployeeId)
        {
            return $"Response from GetEmployeeDetails Method, EmployeeId : {EmployeeId}";
        }
        [Route("{EmployeeName:alpha}")]
        [HttpGet]
        public string GetEmployeeDetails(string EmployeeName)
        {
            return $"Response from GetEmployeeDetails Method, EmployeeName : {EmployeeName}";
        }

        [Route("{Id}")]
        [HttpGet]
        public ActionResult<Employee> GetEmployeeById(int Id)
        {
            
            var employee = _repository.GetById(Id);

            if(employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        [Route("~/api/Student/{gender}/{city}")]
        [HttpGet]
        public ActionResult<Student> GetStudentByGenderAndCity(string gender, string city)
        {
            var filteredStudents = StudentData.Students.Where(e => e.Gender.Equals(gender, StringComparison.OrdinalIgnoreCase) && 
            e.City.Equals(city, StringComparison.OrdinalIgnoreCase)).ToList();

            if(!filteredStudents.Any())
            {
                return NotFound($"No Employee found with Gender '{gender}' in City '{city}'");
            }

            return Ok(filteredStudents);

        }

        [Route("~/api/Student/Search")]
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
        [Route("~/api/Student/Searchs")]
        [HttpGet]
        public ActionResult<IEnumerable<Student>> SearchStudents([FromQuery] StudentSearch searchCriteria)
        {
            var filteredStudents = StudentData.Students.AsQueryable();

            if(!string.IsNullOrEmpty(searchCriteria.Gender))
                filteredStudents = filteredStudents.Where(e => e.Gender.Equals(searchCriteria.Gender, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(searchCriteria.Department))
                filteredStudents = filteredStudents.Where(e => e.Department.Equals(searchCriteria.Department, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(searchCriteria.City))
                filteredStudents = filteredStudents.Where(e => e.City.Equals(searchCriteria.City, StringComparison.OrdinalIgnoreCase));

            var result = filteredStudents.ToList();

            if(!result.Any())
                return NotFound("No employees match the provided search criteria.");

            return Ok(result);
        }

        // GET api/Student/DirectSearch?Gender=Male&Department=IT
        [Route("~/api/Student/DirectSearch")]
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

        [Route("~/api/Student/{gender}")]
        [HttpGet]
        public ActionResult<IEnumerable<Student>> GetStudentsByGender([FromRoute]string gender, [FromQuery] string? department, [FromQuery] string? city)
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

        [HttpPost]
        public ActionResult<Employee> CreateEmployee([FromBody] Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _repository.Add(employee);

            return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.Id }, employee);

        }

        [Route("{id}")]
        [HttpPut]
        public IActionResult updateEmployee(int id, [FromBody] Employee employee)
        {
            if (id != employee.Id)
            {
                return BadRequest("Employ ID Mismatch.");
            }

            if (!_repository.Exists(id))
            {
                return NotFound();
            }

            _repository.Update(employee);

            return NoContent();

        }

        // Partially updates an existing employee (PATCH api/employee/{id}).
        [Route("{id}")]
        [HttpPatch]
        public IActionResult patchEmployee(int id, [FromBody] Employee employee)
        {
            var existingEmployee = _repository.GetById(id);

            if (existingEmployee == null)
            {
                return NotFound();
            }

            // For simplicity, updating all fields. In real scenarios, use JSON Patch.
            existingEmployee.Name = employee.Name ?? employee.Name;
            existingEmployee.Position = employee.Position ?? employee.Position;
            existingEmployee.Age = employee.Age != 0 ? employee.Age : existingEmployee.Age;
            existingEmployee.Email = employee.Email ?? employee.Email;

            _repository.Update(existingEmployee);

            return NoContent();
        }

        // Deletes an employee (DELETE api/employee/{id}).
        [Route("{id}")]
        [HttpDelete]
        public IActionResult deleteEmployee(int id)
        {
            if (!_repository.Exists(id))
            {
                return NotFound();
            }
            _repository.Delete(id);

            return NoContent();
        }
    }
}
