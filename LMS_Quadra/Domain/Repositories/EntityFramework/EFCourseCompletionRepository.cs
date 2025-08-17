using LMS_Quadra.Domain.Entities;
using LMS_Quadra.Domain.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace LMS_Quadra.Domain.Repositories.EntityFramework
{
    public class EFCourseCompletionRepository : ICourseCompletionRepository
    {
        private readonly AppDbContext _context;

        public EFCourseCompletionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetCompletedCount(int workerId)
        {
            return await _context.CourseCompletions
                .CountAsync(cc => cc.WorkerId == workerId && cc.DateCompleted != null);
        }

        public async Task<IEnumerable<CourseCompletion>> GetCourseCompletionAsync()
        {
            return await _context.CourseCompletions.Include(x => x.Worker)
                .Include(x => x.Course)
                .ToListAsync();
        }

        public async Task<CourseCompletion?> GetCourseCompletionByIdAsync(int id)
        {
            return await _context.CourseCompletions.Include(x => x.Worker)
                .Include(x => x.Course)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<double?> GetAverageResult(int workerId)
        {
            return await _context.CourseCompletions
                .Where(cc => cc.WorkerId == workerId && cc.DateCompleted != null)
                .AverageAsync(cc => (double?)cc.Result);
        }

        public async Task SaveCourseCompletionAsync(CourseCompletion entity)
        {
            _context.Entry(entity).State = entity.Id == default ? EntityState.Added : EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCourseCompletionAsync(int id)
        {
            _context.Entry(new CourseCompletion() { Id = id }).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }

        public async Task<CourseCompletion> GetByCourseAndWorkerAsync(int courseId, int workerId)
        {
            return await _context.CourseCompletions
                .Include(cc => cc.Course)
                .FirstOrDefaultAsync(cc =>
                    cc.CourseId == courseId &&
                    cc.WorkerId == workerId);
        }

        public async Task<List<CourseCompletion>> GetByWorkerIdWithCoursesAsync(int workerId, DateTime currentDate)
        {
            return await _context.CourseCompletions
                .Include(cc => cc.Course)
                .Where(cc =>
                    cc.WorkerId == workerId &&
                    cc.DateOpen <= currentDate)
                .ToListAsync();
        }

        public async Task<List<CourseCompletion>> GetUserCompletionsWithCoursesAsync(int workerId)
        {
            return await _context.CourseCompletions
                .Include(cc => cc.Course)
                .Where(cc => cc.WorkerId == workerId && cc.DateCompleted != null)
                .OrderByDescending(cc => cc.DateCompleted)
                .ToListAsync();
        }

        public async Task<double?> GetAverageResultByCourse(int courseId)
        {
            return await _context.CourseCompletions
                .Where(cc => cc.CourseId == courseId && cc.DateCompleted != null)
                .AverageAsync(cc => (double?)cc.Result);
        }

        public async Task<CourseCompletion> GetTopResultByCourse(int courseId)
        {
            return await _context.CourseCompletions
                .Include(cc => cc.Worker)
                .Where(cc => cc.CourseId == courseId && cc.DateCompleted != null)
                .OrderByDescending(cc => cc.Result)
                .ThenBy(cc => cc.DateCompleted)
                .FirstOrDefaultAsync();
        }

        public async Task<int> GetCompletedCountWithDate(int workerId, DateTime startDate, DateTime endDate)
        {
            return await _context.CourseCompletions
                .CountAsync(cc =>
                    cc.WorkerId == workerId &&
                    cc.DateCompleted >= startDate &&
                    cc.DateCompleted <= endDate);
        }
    }
}
