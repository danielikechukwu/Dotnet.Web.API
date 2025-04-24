using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Practical.Web.API.Models;

namespace Practical.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromForm] UserModel user)
        {
            
            if(user.ProfilePicture != null)
            {
                var uploadFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

                if (!Directory.Exists(uploadFolderPath))
                {
                    Directory.CreateDirectory(uploadFolderPath);
                }

                var filePath = Path.Combine(uploadFolderPath, user.ProfilePicture.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await user.ProfilePicture.CopyToAsync(stream);
                }

                // Handle the user data, e.g., save it to a database
                var response = new
                {
                    Success = true,
                    Message = $"User {user.Name} created successfully!",
                    ProfilePictureName = user.ProfilePicture.FileName,
                    Code = StatusCodes.Status200OK
                };

                return Ok(response); 
            }

            return BadRequest(ModelState);
        }

        [HttpGet]
        public IActionResult GetResource([FromHeader] string Authorization)
        {
            //Implementation
            if (Authorization != null)
            {
                return BadRequest("Authorization token is missing");
            }

            return Ok("Request processed successfully");
        }
    }
}
