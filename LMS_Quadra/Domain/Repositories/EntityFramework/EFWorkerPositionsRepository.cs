using LMS_Quadra.Domain.Entities;
using LMS_Quadra.Domain.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace LMS_Quadra.Domain.Repositories.EntityFramework
{
    public class EFWorkerPositionsRepository : IWorkerPositionRepository
    {
        private readonly AppDbContext _context;

        public EFWorkerPositionsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<WorkerPosition>> GetWorkerPositionsAsync() 
        {
            return await _context.WorkerPositions.Include(x=>x.Workers).ToListAsync();
        }

        public async Task<WorkerPosition?> GetWorkerPositionByIdAsync(int id) 
        {
            return await _context.WorkerPositions.Include(x => x.Workers).FirstOrDefaultAsync(x=>x.Id == id);
        }

        public async Task SaveWorkerPositionAsync(WorkerPosition entity) 
        {
            _context.Entry(entity).State = entity.Id == default ? EntityState.Added : EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteWorkerPositionAsync(int id) 
        {
            _context.Entry(new WorkerPosition() { Id = id}).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }
    }
}
