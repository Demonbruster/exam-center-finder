using ExamCenterFinder.Api.Application;

namespace ExamCenterFinder.Api.Infrastructure
{
    public class DistanceCalculatorService : IDistanceCalculatorService
    {
        private readonly ILogger<DistanceCalculatorService> _logger;

        public DistanceCalculatorService(ILogger<DistanceCalculatorService> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// Distance calculator between cordinats  ref:https://gist.github.com/jammin77/033a332542aa24889452
        /// </summary>
        /// <param name="fromLatitude"></param>
        /// <param name="fromLogitude"></param>
        /// <param name="toLatitude"></param>
        /// <param name="toLongitude"></param>
        /// <returns>current distance in miles</returns>
        public async Task<double> CalculateDistance(double fromLatitude, double fromLogitude, double toLatitude, double toLongitude)
        {
            try
            {

                const double EarthRadiusMiles = 3958.8;

                var dLat = ToRadians(toLatitude - fromLatitude);
                var dLon = ToRadians(toLongitude - fromLogitude);

                var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                        Math.Cos(ToRadians(fromLatitude)) * Math.Cos(ToRadians(toLatitude)) *
                        Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

                double c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));

                return EarthRadiusMiles * c;

            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                throw ex; // Re-throw the exception
            }
        }

        private double ToRadians(double degree)
        {
            return degree * Math.PI / 180.0;
        }
    }
}
