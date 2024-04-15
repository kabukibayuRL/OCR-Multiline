using OCRMultiline.Common; 
using Microsoft.AspNetCore.Mvc;

namespace OCRMultiline.Endpoints.TestApi
{
    public class TaskController : BaseController 
    {
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            if (id <= 0)
            {
                return NotFound("ID must be greater than 0");
            }

            if (id == 42)
            {
                return BadRequest("ID 42 is not allowed");
            }

            var data = new { Id = id, Name = "Sample" };
            return Ok(data);
        }
    }
}
