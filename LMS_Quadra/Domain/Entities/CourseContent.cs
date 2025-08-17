using System.ComponentModel.DataAnnotations;

namespace LMS_Quadra.Domain.Entities
{
    public class CourseContent : EntityBase
    {
        [Display(Name = "Курс")]
        public int CourseId { get; set; }
        public Course Course { get; set; }

        [Required(ErrorMessage = "Название темы не заполнено")]
        [Display(Name = "Название темы")]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Поле не заполнено")]
        [Display(Name = "Теоретические материалы")]
        [DataType(DataType.MultilineText)]
        [MaxLength(100000)]
        public string? LinksToTheory { get; set; }
    }
}
