using Microsoft.AspNetCore.Mvc;

namespace ExamCenterFinder.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AvailabilityController : ControllerBase
    {
        private readonly ILogger<AvailabilityController> _logger;
        public AvailabilityController(ILogger<AvailabilityController> logger ) 
        {
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> GetAvailability([FromQuery] int examDuration, [FromQuery] string zipCode, [FromQuery] int distance)
        {
            try
            {
                var validationError = ValidateInputParameters(examDuration, zipCode, distance);
                if (validationError != null)
                    return BadRequest(validationError);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        private string ValidateInputParameters(int examDuration, string zipCode, int distance)
        {
            if (examDuration <= 0)
                return "Invalid input parameter: examDuration must be a positive value.";

            if (string.IsNullOrEmpty(zipCode))
                return "Invalid input parameter: zipCode cannot be null or empty.";

            if (distance <= 0)
                return "Invalid input parameter: distance must be a positive value.";

            return null;
        }
    }
}
