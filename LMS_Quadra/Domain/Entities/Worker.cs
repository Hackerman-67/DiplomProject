using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS_Quadra.Domain.Entities
{
    public class Worker : EntityBase
    {
        [Required(ErrorMessage = "ФИО сотрудника не заполнено")]
        [Display(Name = "ФИО сотрудника")]
        [MaxLength(75)]
        public string Name { get; set; }

        [Column(TypeName = "Date")]
        [Required(ErrorMessage = "Дата рождения не указана")]
        [Display(Name = "Дата рождения")]
        [Range(typeof(DateTime), "1900-01-01", "2100-01-01", ErrorMessage = "Некорректная дата рождения")]
        [DataType(DataType.Date)]
        public DateTime DateBirth { get; set; }

        [Required(ErrorMessage = "Не указан номер телефона")]
        [Display(Name = "Контактный номер телефона")]
        [MaxLength(16)]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Отдел")]
        [Required(ErrorMessage = "Отдел не выбран")]
        public int DepartmentId {  get; set; }
        public Department? Department { get; set; }

        [Display(Name = "Должность")]
        [Required(ErrorMessage = "Должность не выбрана")]
        public int WorkerPositionId { get; set; }
        public WorkerPosition? WorkerPosition { get; set; }

        public ICollection<CourseCompletion>? CourseCompletions { get; set; }

        public string? UserId { get; set; }
        public IdentityUser? User { get; set; }

        /////////// Дополнительные поля, нужные для отчётов (не заносятся в БД):
        [NotMapped]
        public int? CompletedCoursesCount { get; set; }
        [NotMapped]
        public double? AverageResult { get; set; }
    }
}
