using Microsoft.AspNetCore.Mvc;

namespace ExamCenterFinder.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AvailabilityController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAvailability([FromQuery] int examDuration, [FromQuery] string zipCode, [FromQuery] int distance)
        {
            return Ok();
        }
    }
}
