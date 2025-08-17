using LMS_Quadra.Domain.Entities;

namespace LMS_Quadra.Domain.Repositories.Abstract
{
    public interface IWorkerRepository
    {
        Task<IEnumerable<Worker>> GetWorkerAsync();
        Task<IEnumerable<Worker>> GetByDepartmentIdAsync(int departmentId);
        Task<Worker> GetWorkerByIdAsync(int id);
        Task SaveWorkerAsync(Worker entity);
        Task DeleteWorkerAsync(int id);

        Task<Worker> GetByUserIdAsync(string userId);
    }
}
