using ExamCenterFinder.Api.Application;
using ExamCenterFinder.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace ExamCenterFinder.Api.Persistence
{
    public class ExamSlotsRepository : Repository<ExamSlot>, IExamSlotsRepository
    {
        private readonly ExamCenterFinderDbContext _context;
        public ExamSlotsRepository(ExamCenterFinderDbContext context) : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// Get list of slots by duaration and free capacity
        /// </summary>
        /// <param name="duration"></param>
        /// <returns> </returns>
        public async Task<IList<ExamSlot>> GetSlotsByDurationAsync(int duration)
        {
            return await _context.ExamSlots
                .Include(x => x.ExamCenter)
                .ThenInclude(x => x.ZipCodeCenterPoint)
                .Where(es => es.Duration == duration && es.TotalSeats > es.ReservedSeats)
                .ToListAsync();
        }
    }
}
