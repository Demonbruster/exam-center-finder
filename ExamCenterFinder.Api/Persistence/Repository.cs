using ExamCenterFinder.Api.Application;
using Microsoft.EntityFrameworkCore;

namespace ExamCenterFinder.Api.Persistence
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ExamCenterFinderDbContext _context;
        private readonly DbSet<T> _set;

        public Repository(ExamCenterFinderDbContext context)
        {
            _context = context;
            _set = context.Set<T>();
        }

        public async Task<IList<T>> GetAllAsync()
        {
            return await _set.ToListAsync();
        }
    }
}
