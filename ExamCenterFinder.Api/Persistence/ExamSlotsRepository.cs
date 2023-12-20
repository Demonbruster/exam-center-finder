using ExamCenterFinder.Api.Application;
using ExamCenterFinder.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace ExamCenterFinder.Api.Persistence
{
    public class ExamSlotsRepository: Repository<ExamSlot>, IExamSlotsRepository
    {
        private readonly ExamCenterFinderDbContext _context;
        public ExamSlotsRepository(ExamCenterFinderDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IList<ExamSlot>> GetSlotsByDurationAsync(int duration)
        {
            return await _context.ExamSlots.Include(x => x.ExamCenter).ThenInclude(x => x.ZipCodeCenterPoint).Where(es => es.Duration == duration).ToListAsync();
        }
    }
}
