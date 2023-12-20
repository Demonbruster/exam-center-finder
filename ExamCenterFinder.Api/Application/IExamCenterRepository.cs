using ExamCenterFinder.Api.Domain;

namespace ExamCenterFinder.Api.Application
{
    public interface IExamCenterRepository: IRepository<ExamCenter>
    {
        /// <summary>
        /// Get list of exam centers with available slots on given durations
        /// </summary>
        /// <param name="duration"></param>
        /// <returns></returns>
        Task<IList<ExamCenter>> GetAvailableExamCenters(int duration);
    }
}
