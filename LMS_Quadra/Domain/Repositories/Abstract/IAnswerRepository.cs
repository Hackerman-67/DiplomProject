using LMS_Quadra.Domain.Entities;

namespace LMS_Quadra.Domain.Repositories.Abstract
{
    public interface IAnswerRepository
    {
        Task<IEnumerable<Answer>> GetAnswersAsync();
        Task<Answer> GetAnswerByIdAsync(int id);
        Task SaveAnswerAsync(Answer entity);
        Task DeleteAnswerAsync(int id);
    }
}
