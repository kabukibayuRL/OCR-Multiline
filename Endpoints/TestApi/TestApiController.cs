using OCRMultiline.Common; 
using Microsoft.AspNetCore.Mvc;

namespace OCRMultiline.Endpoints.TestApi
{
    public class TestApiController : BaseController 
    {
        // Example endpoint using common handlers
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            // Example: Handle not found scenario
            if (id <= 0)
            {
                return NotFound("ID must be greater than 0");
            }

            // Example: Handle bad request scenario
            if (id == 42)
            {
                return BadRequest("ID 42 is not allowed");
            }

            // Example: Handle OK scenario
            var data = new { Id = id, Name = "Sample" };
            return Ok(data);
        }
    }
}
