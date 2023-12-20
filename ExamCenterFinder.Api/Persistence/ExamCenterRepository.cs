using ExamCenterFinder.Api.Application;
using ExamCenterFinder.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace ExamCenterFinder.Api.Persistence
{
    public class ExamCenterRepository: Repository<ExamCenter>, IExamCenterRepository
    {
        private readonly ExamCenterFinderDbContext _context;
        public ExamCenterRepository(ExamCenterFinderDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IList<ExamCenter>> GetAvailableExamCenters(int duration)
        {
            return await _context.ExamCenters
           .Where(ec => ec.ExamSlots.Any(es => es.Duration == duration && es.ReservedSeats < es.TotalSeats))
           .ToListAsync();
        }
    }
}
