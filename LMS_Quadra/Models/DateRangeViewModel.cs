using System.ComponentModel.DataAnnotations;

namespace LMS_Quadra.Models
{
    public class DateRangeViewModel
    {
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Дата начала периода")]
        public DateTime StartDate { get; set; } = DateTime.Now.AddMonths(-1);

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Дата конца периода")]
        public DateTime EndDate { get; set; } = DateTime.Now;
    }
}
