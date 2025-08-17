using LMS_Quadra.Domain.Entities;
using LMS_Quadra.Domain.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace LMS_Quadra.Domain.Repositories.EntityFramework
{
    public class EFAnswerRepository : IAnswerRepository
    {
        private readonly AppDbContext _context;

        public EFAnswerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Answer>> GetAnswersAsync()
        {
            return await _context.Answers.Include(x => x.Question)
                .ToListAsync();
        }

        public async Task<Answer?> GetAnswerByIdAsync(int id)
        {
            return await _context.Answers.Include(x => x.Question)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task SaveAnswerAsync(Answer entity)
        {
            _context.Entry(entity).State = entity.Id == default ? EntityState.Added : EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAnswerAsync(int id)
        {
            _context.Entry(new Answer() { Id = id }).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }
    }
}
