using ExamCenterFinder.Api.Domain.Dtos;

namespace ExamCenterFinder.Api.Application.Services
{
    public class AvailabilityService : IAvailabilityService
    {
        private readonly IZipCodeCenterPointRepository _zipCodeCenterPointRepository;
        private readonly IDistanceCalculatorService _distanceCalculatorService;
        private readonly IExamSlotsRepository _examSlotsRepository;
        private readonly ILogger<AvailabilityService> _logger;

        public AvailabilityService(IZipCodeCenterPointRepository zipCodeCenterPointRepository, IDistanceCalculatorService distanceCalculatorService, IExamSlotsRepository examSlotsRepository, ILogger<AvailabilityService> logger)
        {
            _zipCodeCenterPointRepository = zipCodeCenterPointRepository;
            _distanceCalculatorService = distanceCalculatorService;
            _examSlotsRepository = examSlotsRepository;
            _logger = logger;
        }
        public async Task<IList<ExamCenterDto>> GetAvailalbleExamCenters(int examDuration, string zipCode, int distance)
        {
            try
            {
                var userZipCodeCenterpoint = await _zipCodeCenterPointRepository.GetZipCodeCenterPointsByZipCode(zipCode);
                if (userZipCodeCenterpoint == null) throw InvalidOperationException("ZipCode data not found");
                var examSlots = await _examSlotsRepository.GetSlotsByDurationAsync(examDuration);
                var examCenterDtos = await Task.WhenAll(examSlots.Select(async es =>
                {
                    var distanceMiles = await _distanceCalculatorService.CalculateDistance(
                        userZipCodeCenterpoint.Latitude,
                        userZipCodeCenterpoint.Longitude,
                        es.ExamCenter.ZipCodeCenterPoint.Latitude,
                        es.ExamCenter.ZipCodeCenterPoint.Longitude
                    );

                    return new ExamCenterDto
                    {
                        AvailabilityId = GenerateAvailabilityId(),
                        Name = es.ExamCenter.Name,
                        Address = es.ExamCenter.StreetAddress,
                        StartTime = es.StartTime,
                        Seats = es.TotalSeats - es.ReservedSeats,
                        Latitude = es.ExamCenter.ZipCodeCenterPoint.Latitude,
                        Longitude = es.ExamCenter.ZipCodeCenterPoint.Longitude,
                        DistanceMiles = distanceMiles
                    };
                }));

                // Filter by distance
                return examCenterDtos.Where(av => av.DistanceMiles <= distance).OrderBy(av => av.DistanceMiles).ToList();

            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                throw ex;
            }
        }

        private Exception InvalidOperationException(string v)
        {
            throw new NotImplementedException();
        }

        private int GenerateAvailabilityId()
        {
            // Replace this with your logic to generate AvailabilityId
            return new Random().Next(1000, 9999);
        }
    }
}
