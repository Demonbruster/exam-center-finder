﻿using ExamCenterFinder.Api.Application;
using ExamCenterFinder.Api.Domain.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace ExamCenterFinder.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AvailabilityController : ControllerBase
    {
        private readonly ILogger<AvailabilityController> _logger;
        private readonly IAvailabilityService _availabilityService;

        public AvailabilityController(ILogger<AvailabilityController> logger, IAvailabilityService availabilityService ) 
        {
            _logger = logger;
            _availabilityService = availabilityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAvailability([FromQuery] int examDuration, [FromQuery] string zipCode, [FromQuery] int distanceInMiles)
        {
            try
            {
                var validationError = ValidateInputParameters(examDuration, zipCode, distanceInMiles);
                if (!String.IsNullOrEmpty(validationError))
                    return BadRequest(validationError);

                var availabilities = await _availabilityService.GetAvailalbleExamCentersAsync(examDuration, zipCode, distanceInMiles);

                if (availabilities == null || availabilities.Count == 0) 
                    return NotFound("There no available centers matching the request");

                return Ok(new AvailablitiesDto
                {
                    Availability = availabilities
                });
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogWarning($"An error occurred: {ioe.Message}");
                return BadRequest(ioe.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
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

            return String.Empty;
        }
    }
}
