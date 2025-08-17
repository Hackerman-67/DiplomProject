using LMS_Quadra.Domain.Entities;
namespace LMS_Quadra.Domain.Repositories.Abstract
{
    public interface IWorkerPositionRepository
    {
        Task<IEnumerable<WorkerPosition>> GetWorkerPositionsAsync();
        Task<WorkerPosition> GetWorkerPositionByIdAsync(int id);
        Task SaveWorkerPositionAsync(WorkerPosition entity);
        Task DeleteWorkerPositionAsync(int id);
    }
}
