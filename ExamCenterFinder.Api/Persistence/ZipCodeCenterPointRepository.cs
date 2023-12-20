using ExamCenterFinder.Api.Application;
using ExamCenterFinder.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace ExamCenterFinder.Api.Persistence
{
    public class ZipCodeCenterPointRepository: Repository<ZipCodeCenterPoint>, IZipCodeCenterPointRepository
    {
        private readonly ExamCenterFinderDbContext _context;
        public ZipCodeCenterPointRepository(ExamCenterFinderDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ZipCodeCenterPoint> GetZipCodeCenterPointsByZipCodeAsync(string zipCode)
        {
            return await _context.ZipCodeCenterPoints.Where(ec => ec.ZipCode == zipCode).FirstOrDefaultAsync();
        }
    }
}
