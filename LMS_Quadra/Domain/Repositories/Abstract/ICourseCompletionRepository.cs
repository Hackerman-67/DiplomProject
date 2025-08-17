using LMS_Quadra.Domain.Entities;

namespace LMS_Quadra.Domain.Repositories.Abstract
{
    public interface ICourseCompletionRepository
    {
        Task<IEnumerable<CourseCompletion>> GetCourseCompletionAsync();
        Task<double?> GetAverageResult(int workerId);
        Task<double?> GetAverageResultByCourse(int courseId);
        Task<CourseCompletion> GetCourseCompletionByIdAsync(int id);
        Task SaveCourseCompletionAsync(CourseCompletion entity);
        Task DeleteCourseCompletionAsync(int id);
        Task<CourseCompletion> GetByCourseAndWorkerAsync(int courseId, int workerId);
        Task<CourseCompletion> GetTopResultByCourse(int courseId);
        Task<List<CourseCompletion>> GetByWorkerIdWithCoursesAsync(int workerId, DateTime currentDate);
        Task<List<CourseCompletion>> GetUserCompletionsWithCoursesAsync(int workerId);
        Task<int> GetCompletedCount(int workerId);
        Task<int> GetCompletedCountWithDate(int workerId, DateTime startDate, DateTime endDate);

    }
}
