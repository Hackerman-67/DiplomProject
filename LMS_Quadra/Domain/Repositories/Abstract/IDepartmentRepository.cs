using LMS_Quadra.Domain.Entities;
namespace LMS_Quadra.Domain.Repositories.Abstract
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> GetDepartmentAsync();
        Task<Department> GetDepartmentByIdAsync(int id);
        Task SaveDepartmentAsync(Department entity);
        Task DeleteDepartmentAsync(int id);
        Task<List<Department>> GetDepartmentReport(DateTime startDate, DateTime endDate);
    }
}
