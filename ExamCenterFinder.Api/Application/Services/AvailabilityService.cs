using ExamCenterFinder.Api.Domain.Dtos;

namespace ExamCenterFinder.Api.Application.Services
{
    public class AvailabilityService : IAvailabilityService
    {
        private readonly IDistanceCalculatorService _distanceCalculatorService;
        private readonly IExamSlotsRepository _examSlotsRepository;
        private readonly ILogger<AvailabilityService> _logger;

        public AvailabilityService(IDistanceCalculatorService distanceCalculatorService, IExamSlotsRepository examSlotsRepository, ILogger<AvailabilityService> logger)
        {
            _distanceCalculatorService = distanceCalculatorService;
            _examSlotsRepository = examSlotsRepository;
            _logger = logger;
        }
        public async Task<IList<ExamCenterDto>> GetAvailalbleExamCenters(int examDuration, string zipCode, int distance)
        {
            try
            {
                var availableExamCenterDtos = new List<ExamCenterDto>();
                var distanceMile = await _distanceCalculatorService.CalculateDistance(zipCode, distance);
                var examSlots = await _examSlotsRepository.GetSlotsByDurationAsync(examDuration);

                availableExamCenterDtos = examSlots.Select(es => new ExamCenterDto
                {
                    AvailabilityId = GenerateAvailabilityId(),
                    Name = es.ExamCenter.Name,
                    Address = es.ExamCenter.StreetAddress,
                    StartTime = es.StartTime,
                    Seats = es.TotalSeats - es.ReservedSeats,
                    Latitude = es.ExamCenter.ZipCodeCenterPoint.Latitude,
                    Longitude = es.ExamCenter.ZipCodeCenterPoint.Longitude,
                    DistanceMiles = distanceMile
                }).ToList();

                return availableExamCenterDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                throw ex;
            }
        }

        private int GenerateAvailabilityId()
        {
            // Replace this with your logic to generate AvailabilityId
            return new Random().Next(1000, 9999);
        }
    }
}
