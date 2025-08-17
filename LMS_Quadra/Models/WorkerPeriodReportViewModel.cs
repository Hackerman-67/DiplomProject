using System.ComponentModel.DataAnnotations;

namespace LMS_Quadra.Models
{
    public class WorkerPeriodReportViewModel
    {
        [Required(ErrorMessage = "Выберите сотрудника")]
        [Display(Name = "Сотрудник")]
        public int WorkerId { get; set; }

        [Required(ErrorMessage = "Укажите дату начала")]
        [DataType(DataType.Date)]
        [Display(Name = "Дата начала периода")]
        public DateTime StartDate { get; set; } = DateTime.Now.AddMonths(-1);

        [Required(ErrorMessage = "Укажите дату окончания")]
        [DataType(DataType.Date)]
        [Display(Name = "Дата конца периода")]
        public DateTime EndDate { get; set; } = DateTime.Now;
    }
}
