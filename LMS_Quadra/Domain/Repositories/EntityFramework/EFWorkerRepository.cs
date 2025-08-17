using LMS_Quadra.Domain.Entities;
using LMS_Quadra.Domain.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace LMS_Quadra.Domain.Repositories.EntityFramework
{
    public class EFWorkerRepository : IWorkerRepository
    {
        private readonly AppDbContext _context;

        public EFWorkerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Worker>> GetWorkerAsync()
        {
            return await _context.Workers.Include(x => x.WorkerPosition)
                .Include(x=>x.Department)
                .Include(x=>x.CourseCompletions)
                .Include(x=>x.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<Worker>> GetByDepartmentIdAsync(int departmentId)
        {
            return await _context.Workers.Where(w=>w.DepartmentId == departmentId).ToListAsync();
        }

        public async Task<Worker?> GetWorkerByIdAsync(int id)
        {
            return await _context.Workers.Include(x => x.WorkerPosition)
                .Include(x => x.Department)
                .Include(x => x.CourseCompletions)
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Worker> GetByUserIdAsync(string userId)
        {
            return await _context.Workers.FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task SaveWorkerAsync(Worker entity)
        {
            _context.Entry(entity).State = entity.Id == default ? EntityState.Added : EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteWorkerAsync(int id)
        {
            _context.Entry(new Worker() { Id = id }).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }
    }
}
