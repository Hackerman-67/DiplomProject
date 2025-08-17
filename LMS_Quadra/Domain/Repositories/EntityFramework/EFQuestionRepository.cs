using LMS_Quadra.Domain.Entities;
using LMS_Quadra.Domain.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace LMS_Quadra.Domain.Repositories.EntityFramework
{
    public class EFQuestionRepository : IQuestionRepository
    {
        private readonly AppDbContext _context;

        public EFQuestionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Question>> GetQuestionsAsync()
        {
            return await _context.Questions.Include(x => x.Course)
                .Include(x => x.Answers)
                .ToListAsync();
        }

        public async Task<IEnumerable<Question>> GetQuestionsByCourseIdAsync(int courseId) 
        {
            return await _context.Questions
            .Where(cc => cc.CourseId == courseId)
            .ToListAsync();
        }

        public async Task<Question?> GetQuestionByIdAsync(int id)
        {
            return await _context.Questions.Include(x => x.Course)
                .Include(x => x.Answers)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task SaveQuestionAsync(Question entity)
        {
            _context.Entry(entity).State = entity.Id == default ? EntityState.Added : EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteQuestionAsync(int id)
        {
            _context.Entry(new Question() { Id = id }).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }

        public async Task<List<Question>> GetRandomQuestionsAsync(int courseId, int count)
        {
            return await _context.Questions
            .Where(q => q.CourseId == courseId)
            .Include(q => q.Answers)
            .OrderBy(q => Guid.NewGuid())
            .Take(count)
            .ToListAsync();
        }

        public async Task<List<Question>> GetQuestionsByIdsAsync(List<int> ids)
        {
            return await _context.Questions
                .Where(q => ids.Contains(q.Id))
                .Include(q => q.Answers)
                .ToListAsync();
        }
    }
}
