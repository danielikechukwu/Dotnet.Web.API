using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Practical.Web.API.Models;

namespace Practical.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SampleController : ControllerBase
    {
        //Limited to Built-in Sources
        //Limitation:
        //The model binder can’t bind CustomObject directly from a custom string format like "Name:Age:Location".
        [HttpGet("custom-object-binding")]
        public IActionResult CustomObjectBinding([FromQuery] string complexData)
        {
            // The data is in the custom format "Name:Age:Location"
            var parts = complexData?.Split(':');
            if (parts?.Length == 3)
            {
                var customObject = new CustomObject
                {
                    Name = parts[0],
                    Age = int.Parse(parts[1]),
                    Location = parts[2]
                };
                return Ok(customObject);
            }
            return BadRequest("Invalid custom format");
        }

        //Lacks Flexibility
        //Limitation:
        //The default model binder cannot easily handle merging data from multiple sources(headers, query, and body) into a single model.
        [HttpPost("multiple-source-binding")]
        public IActionResult MultipleSourceBinding([FromHeader(Name = "X-Custom-Header")] string headerValue, [FromQuery] string queryValue, [FromBody] ComplexBodyModel bodyModel)
        {
            // Merging data from header, query, and body
            var mergeValue = new MergedModel
            {
                Header = headerValue,
                Query = queryValue,
                BodyData = bodyModel.Data
            };

            return Ok(mergeValue);

        }

        //No Support for Special Data Types
        //Limitation:
        //Tuples cannot be easily bound by the default model binder without additional configuration or custom binding logic.
        [HttpPost("tuple-binding")]
        public IActionResult TupleBinding([FromBody] CustomTupleModel model)
        {
            return Ok($"Tuple Data: Item1 = {model.Item1}, Item2 = {model.Item2}");
        }

        //Performance Issues for Large Data
        //Limitation:
        //For large payloads, FromBody reads the entire request body into memory, which can be inefficient and lead to performance issues.
        [HttpPost("large-data-binding")]
        public IActionResult LargeDataBind([FromBody] LargeDataModel model)
        {
            // Assume the data size is very large
            return Ok("Data received successfully");
        }
    }
}
