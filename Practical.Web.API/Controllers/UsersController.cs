using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Practical.Web.API.Models;

namespace Practical.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpPost]
        public IActionResult CreateUser([FromForm] UserModel user)
        {
            // Handle the user data, e.g., save it to a database
            var response = new
            {
                Success = true,
                Message = $"User {user.Name} created successfully",
                Code = StatusCodes.Status200OK
            };

            return Ok(response);
        }
    }
}
