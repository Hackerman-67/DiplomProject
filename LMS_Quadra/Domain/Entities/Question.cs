using System.ComponentModel.DataAnnotations;

namespace LMS_Quadra.Domain.Entities
{
    public class Question : EntityBase
    {
        [Display(Name = "Курс")]
        public int CourseId { get; set; }
        public Course? Course { get; set; }

        [Required(ErrorMessage = "Вопрос не заполнен")]
        [Display(Name = "Формулировка вопроса")]
        [MaxLength(200)]
        public string Wording { get; set; }

        public ICollection<Answer>? Answers { get; set; } = new List<Answer>();
    }
}
