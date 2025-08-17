using LMS_Quadra.Domain.Repositories.Abstract;

namespace LMS_Quadra.Domain
{
    public class DataManager
    {
        public IAnswerRepository AnswerRepository { get; set; }
        public ICourseCompletionRepository CourseCompletionRepository { get; set; }
        public ICourseContentRepository CourseContentRepository { get; set; }
        public ICourseRepository CourseRepository { get; set; }
        public IDepartmentRepository DepartmentRepository { get; set; }
        public IQuestionRepository QuestionRepository { get; set; }
        public IWorkerPositionRepository WorkerPositionRepository { get; set; }
        public IWorkerRepository WorkerRepository { get; set; }

        public DataManager(IAnswerRepository answerRepository, 
            ICourseCompletionRepository courseCompletionRepository, 
            ICourseContentRepository contentRepository, 
            ICourseRepository courseRepository, 
            IDepartmentRepository departmentRepository, 
            IQuestionRepository questionRepository, 
            IWorkerPositionRepository workerPositionRepository, 
            IWorkerRepository workerRepository)
        {
            AnswerRepository = answerRepository;
            CourseCompletionRepository = courseCompletionRepository;
            CourseContentRepository = contentRepository;
            CourseRepository = courseRepository;
            DepartmentRepository = departmentRepository;
            QuestionRepository = questionRepository;
            WorkerPositionRepository = workerPositionRepository;
            WorkerRepository = workerRepository;
        }
    }
}
