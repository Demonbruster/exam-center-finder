namespace ExamCenterFinder.Api.Application.Services
{
    public class DistanceCalculatorService : IDistanceCalculatorService
    {
        private readonly IZipCodeCenterPointRepository _zipCodeCenterPointRepository;
        private readonly ILogger<DistanceCalculatorService> _logger;

        public DistanceCalculatorService(IZipCodeCenterPointRepository zipCodeCenterPointRepository, ILogger<DistanceCalculatorService> logger)
        {
            _zipCodeCenterPointRepository = zipCodeCenterPointRepository;
            _logger = logger;
        }
        public async Task<double> CalculateDistance(string zipCode, int distance)
        {
            try
            {
                var zipCodeData = await _zipCodeCenterPointRepository.GetZipCodeCenterPointsByZipCode(zipCode);
                if (zipCodeData == null)
                {
                    var errorMessage = $"Zip code not found: {zipCode}";
                    _logger.LogWarning(errorMessage);
                    throw new InvalidOperationException(errorMessage);
                }
                    return DistanceBetweenCoordinates(zipCodeData.Latitude, zipCodeData.Longitude, distance);

            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                throw ex; // Re-throw the exception
            }
        }

        /// <summary>
        /// Distance calculator between cordinats  ref:https://gist.github.com/jammin77/033a332542aa24889452
        /// </summary>
        /// <param name="lat1"> latitue</param>
        /// <param name="lon1"> longitue</param>
        /// <param name="distance"> distance in miles</param>
        /// <returns>current distance in miles</returns>
        private double DistanceBetweenCoordinates(double lat1, double lon1, int distance)
        {
            try
            {
                const double EarthRadiusMiles = 3958.8;

                var dLat = ToRadians(distance - lat1);
                var dLon = ToRadians(distance - lon1);

                var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                        Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(distance)) *
                        Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

                var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

                return EarthRadiusMiles * c;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw ex;
            }
        }

        private double ToRadians(double degree)
        {
            return degree * Math.PI / 180.0;
        }
    }
}
