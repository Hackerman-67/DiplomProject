using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS_Quadra.Domain.Entities
{
    public class CourseCompletion : EntityBase
    {
        [Display(Name = "Курс")]
        public int CourseId { get; set; }
        public Course? Course { get; set; }

        [Display(Name = "Сотрудник")]
        public int WorkerId { get; set; }
        public Worker? Worker { get; set; }

        [Column(TypeName = "Date")]
        [Required(ErrorMessage = "Дата открытия не указана")]
        [Display(Name = "Дата открытия курса")]
        [DataType(DataType.Date)]
        public DateTime DateOpen { get; set; }

        [Column(TypeName = "Date")]
        [Display(Name = "Дата прохождения курса")]
        public DateTime? DateCompleted { get; set; }

        [Display(Name = "Результат прохождения курса")]
        [Column(TypeName = "decimal(5, 2)")]
        public decimal? Result { get; set; }
    }
}
