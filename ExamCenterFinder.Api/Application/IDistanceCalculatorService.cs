namespace ExamCenterFinder.Api.Application
{
    public interface IDistanceCalculatorService
    {
        Task<double> CalculateDistance(string zipCode, int distance);
    }
}
