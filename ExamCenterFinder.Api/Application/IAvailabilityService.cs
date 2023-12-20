using ExamCenterFinder.Api.Domain.Dtos;

namespace ExamCenterFinder.Api.Application
{
    public interface IAvailabilityService
    {
        Task<IList<ExamCenterDto>> GetAvailalbleExamCenters(int examDuration, string zipCode, int distance);
    }
}
