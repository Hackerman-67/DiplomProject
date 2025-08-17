using System.ComponentModel.DataAnnotations;

namespace LMS_Quadra.Domain.Entities
{
    public class WorkerPosition : EntityBase
    {
        [Required(ErrorMessage = "Название должности не заполнено")]
        [Display(Name = "Название должности")]
        [MaxLength(50)]
        public string Title { get; set; }

        public ICollection<Worker>? Workers { get; set; }

    }
}
