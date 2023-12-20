using ExamCenterFinder.Api.Domain;

namespace ExamCenterFinder.Api.Application
{
    public interface IExamSlotsRepository: IRepository<ExamSlot>
    {
        /// <summary>
        /// Get list of slots by the duration.
        /// </summary>
        /// <param name="duration"> exam duration </param>
        /// <returns></returns>
        Task<IList<ExamSlot>> GetSlotsByDurationAsync(int duration);
    }
}
