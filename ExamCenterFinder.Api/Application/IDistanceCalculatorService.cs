namespace ExamCenterFinder.Api.Application
{
    public interface IDistanceCalculatorService
    {
        Task<double> CalculateDistance(double fromLatitude, double fromLogitude, double toLatitude, double toLongitude);
    }
}
