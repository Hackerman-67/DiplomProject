using LMS_Quadra.Domain.Entities;

namespace LMS_Quadra.Domain.Repositories.Abstract
{
    public interface IQuestionRepository
    {
        Task<IEnumerable<Question>> GetQuestionsAsync();
        Task<IEnumerable<Question>> GetQuestionsByCourseIdAsync(int courseId);
        Task<Question> GetQuestionByIdAsync(int id);
        Task SaveQuestionAsync(Question entity);
        Task DeleteQuestionAsync(int id);
        Task<List<Question>> GetQuestionsByIdsAsync(List<int> ids);
        Task<List<Question>> GetRandomQuestionsAsync(int courseId, int count);


    }
}
