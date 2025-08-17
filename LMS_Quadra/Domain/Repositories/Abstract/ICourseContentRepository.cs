using LMS_Quadra.Domain.Entities;

namespace LMS_Quadra.Domain.Repositories.Abstract
{
    public interface ICourseContentRepository
    {
        Task<IEnumerable<CourseContent>> GetCourseContentAsync();
        Task<IEnumerable<CourseContent>> GetCourseContentByCourseIdAsync(int courseId);
        Task<CourseContent> GetCourseContentByIdAsync(int id);
        Task SaveCourseContentAsync(CourseContent entity);
        Task DeleteCourseContentAsync(int id);
    }
}
