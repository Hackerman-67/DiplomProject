using LMS_Quadra.Domain.Entities;

namespace LMS_Quadra.Domain.Repositories.Abstract
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Course>> GetCourseAsync();
        Task<Course> GetCourseByIdAsync(int id);
        Task SaveCourseAsync(Course entity);
        Task DeleteCourseAsync(int id);
    }
}
