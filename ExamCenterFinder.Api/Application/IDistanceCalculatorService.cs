namespace ExamCenterFinder.Api.Application
{
    public interface IDistanceCalculatorService
    {
        Task<double> CalculateDistanceAsync(double fromLatitude, double fromLogitude, double toLatitude, double toLongitude);
    }
}
