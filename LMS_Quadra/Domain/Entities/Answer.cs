using System.ComponentModel.DataAnnotations;

namespace LMS_Quadra.Domain.Entities
{
    public class Answer : EntityBase
    {
        [Display(Name = "Вопрос")]
        public int QuestionId { get; set; }
        public Question? Question { get; set; }

        [Required(ErrorMessage = "Ответ не заполнен")]
        [Display(Name = "Формулировка ответа")]
        [MaxLength(200)]
        public string Wording { get; set; }

        [Required(ErrorMessage = "Корректность ответа не указана")]
        [Display(Name = "Правильный ответ")]
        public bool IsCorrect { get; set; }
    }
}
