using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS_Quadra.Domain.Entities
{
    public class Department : EntityBase
    {
        [Required(ErrorMessage = "Наименование отдела не заполнено")]
        [Display(Name = "Наименование отдела")]
        [MaxLength(75)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Не указан руководитель отдела")]
        [Display(Name = "Руководитель отдела")]
        [MaxLength(75)]
        public string Head { get; set; }

        [Required(ErrorMessage = "Не указан номер телефона")]
        [Display(Name = "Контактный номер телефона")]
        [MaxLength(16)]
        public string PhoneNumber { get; set; }

        public ICollection<Worker>? Workers { get; set; }

        [NotMapped]
        public int CompletedCoursesCount { get; set; }
    }
}
