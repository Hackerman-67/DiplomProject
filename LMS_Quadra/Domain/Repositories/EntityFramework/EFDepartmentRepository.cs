using LMS_Quadra.Domain.Entities;
using LMS_Quadra.Domain.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace LMS_Quadra.Domain.Repositories.EntityFramework
{
        public class EFDepartmentRepository : IDepartmentRepository
        {
            private readonly AppDbContext _context;

            public EFDepartmentRepository(AppDbContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<Department>> GetDepartmentAsync()
            {
                return await _context.Departments.Include(x => x.Workers).ToListAsync();
            }

            public async Task<Department?> GetDepartmentByIdAsync(int id)
            {
                return await _context.Departments.Include(x => x.Workers).FirstOrDefaultAsync(x => x.Id == id);
            }

            public async Task SaveDepartmentAsync(Department entity)
            {
                _context.Entry(entity).State = entity.Id == default ? EntityState.Added : EntityState.Modified;
                await _context.SaveChangesAsync();
            }

            public async Task DeleteDepartmentAsync(int id)
            {
                _context.Entry(new Department() { Id = id }).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
            }

            public async Task<List<Department>> GetDepartmentReport(DateTime startDate, DateTime endDate)
            {
                var departments = await _context.Departments
                    .Include(d => d.Workers)
                        .ThenInclude(w => w.CourseCompletions)
                    .ToListAsync();

                foreach (var department in departments)
                {
                    department.CompletedCoursesCount = department.Workers
                        .SelectMany(w => w.CourseCompletions)
                        .Count(cc => cc.DateCompleted >= startDate &&
                               cc.DateCompleted <= endDate);
                }

                return departments.OrderByDescending(d => d.CompletedCoursesCount).ToList();
            }
        }
    }
