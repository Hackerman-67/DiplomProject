using System.ComponentModel.DataAnnotations;

namespace LMS_Quadra.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Логин")]
        public string? UserName { get; set; }

        [Required]
        [UIHint("password")]
        [Display(Name = "Пароль")]
        public string? Password { get; set; }
    }
}
