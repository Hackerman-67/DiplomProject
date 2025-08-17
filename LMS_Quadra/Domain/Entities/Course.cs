using System.ComponentModel.DataAnnotations;

namespace LMS_Quadra.Domain.Entities
{
    public class Course : EntityBase
    {
        [Required(ErrorMessage = "Наименование курса не заполнено")]
        [Display(Name = "Наименование курса")]
        [MaxLength(75)]
        public string Title { get; set; }

        [Display(Name = "Описание курса")]
        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Не указано количество вопросов")]
        [Display(Name = "Количество вопросов в тесте")]
        public int NumQuestions { get; set; }

        [Required(ErrorMessage = "Не указан минимальный балл для прохождения курса")]
        [Display(Name = "Минимальный балл")]
        public double MinResult { get; set; }

        public ICollection<CourseCompletion>? CourseCompletions { get; set; }

        public ICollection<CourseContent>? CourseContents { get; set; }
        public ICollection<Question>? Questions { get; set; }
    }
}
