using Microsoft.AspNetCore.Mvc;

namespace OCRMultiline.Common
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
        protected IActionResult NotFoundHandler(string message = "Resource not found")
        {
            return NotFound(new { message });
        }

        protected IActionResult BadRequestHandler(string message = "Bad request")
        {
            return BadRequest(new { message });
        }

        protected IActionResult OkHandler(object data = null)
        {
            return Ok(new { data });
        }
    }
}
