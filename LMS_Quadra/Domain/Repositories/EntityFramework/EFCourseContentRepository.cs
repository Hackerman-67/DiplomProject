using LMS_Quadra.Domain.Entities;
using LMS_Quadra.Domain.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace LMS_Quadra.Domain.Repositories.EntityFramework
{
    public class EFCourseContentRepository : ICourseContentRepository
    {
        private readonly AppDbContext _context;

        public EFCourseContentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CourseContent>> GetCourseContentAsync()
        {
            return await _context.CourseContents.Include(x => x.Course)
                .ToListAsync();
        }

        public async Task<CourseContent?> GetCourseContentByIdAsync(int id)
        {
            return await _context.CourseContents.Include(x => x.Course)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<CourseContent>> GetCourseContentByCourseIdAsync(int courseId) 
        {
            return await _context.CourseContents
            .Where(cc => cc.CourseId == courseId)
            .ToListAsync();
        }

        public async Task SaveCourseContentAsync(CourseContent entity)
        {
            _context.Entry(entity).State = entity.Id == default ? EntityState.Added : EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCourseContentAsync(int id)
        {
            _context.Entry(new CourseContent() { Id = id }).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }
    }
}
