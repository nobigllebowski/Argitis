using System.ComponentModel.DataAnnotations;

namespace Argitis.Models
{
    public class ContactFormModel
    {
        [Display(Name = "Ваше полное имя")]
        [Required(ErrorMessage = "Введите ваше имя")]
        [StringLength(100, ErrorMessage = "Максимум 100 символов")]
        public string Name { get; set; } = "";

        [Display(Name = "Ваш email")]
        [Required(ErrorMessage = "Введите ваш email")]
        [EmailAddress(ErrorMessage = "Введите корректный email")]
        public string Email { get; set; } = "";

        [Display(Name = "Ваш телефон")]
        [Required(ErrorMessage = "Введите ваш телефон")]
        [Phone(ErrorMessage = "Введите корректный телефон")]
        public string Phone { get; set; } = "";

        [Display(Name = "Ваше сообщение")]
        [Required(ErrorMessage = "Введите ваше сообщение")]
        [StringLength(500, ErrorMessage = "Максимум 500 символов")]
        public string Message { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
