using LMS_Quadra.Domain.Entities;
using LMS_Quadra.Domain.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace LMS_Quadra.Domain.Repositories.EntityFramework
{
    public class EFCourseRepository : ICourseRepository
    {
        private readonly AppDbContext _context;

        public EFCourseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Course>> GetCourseAsync()
        {
            return await _context.Courses.Include(x => x.CourseCompletions)
                .Include(x => x.CourseContents)
                .Include(x => x.Questions)
                .ToListAsync();
        }

        public async Task<Course?> GetCourseByIdAsync(int id)
        {
            return await _context.Courses.Include(x => x.CourseCompletions)
                .Include(x => x.CourseContents)
                .Include(x => x.Questions)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task SaveCourseAsync(Course entity)
        {
            _context.Entry(entity).State = entity.Id == default ? EntityState.Added : EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCourseAsync(int id)
        {
            _context.Entry(new Course() { Id = id }).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }
    }
}
