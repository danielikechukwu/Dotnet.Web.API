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

        [HttpGet]
        public ActionResult<IEnumerable<Employee>> GetAllEmployees()
        {
            var employees = _repository.GetAll();

            return Ok(employees);

        }

        [HttpGet("{Id}")]
        public ActionResult<Employee> GetEmployeeById(int Id)
        {
            
            var employee = _repository.GetById(Id);

            if(employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        [HttpGet("{gender}/{city}")]
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

        [HttpPut("{id}")]
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
        [HttpPatch("{id}")]
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
        [HttpDelete("{id}")]
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
