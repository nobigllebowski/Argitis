using System.ComponentModel.DataAnnotations;

namespace Argitis.Models
{
    public class LoanRequest
    {
        [Display(Name = "Желаемая сумма")]
        [Required(ErrorMessage = "Введите желаемую сумму")]
        [Range(1000, 1000000, ErrorMessage = "Сумма должна быть от 1000 до 1 000 000")]
        public decimal Amount { get; set; }

        [Display(Name = "Валюта")]
        [Required(ErrorMessage = "Выберите валюту")]
        public string Currency { get; set; } = "EUR";

        [Display(Name = "Срок (месяцев)")]
        [Required(ErrorMessage = "Введите срок кредита")]
        [Range(1, 360, ErrorMessage = "Срок от 1 до 360 месяцев")]
        public int Period { get; set; }

        [Display(Name = "Фамилия и имя")]
        [Required(ErrorMessage = "Введите ваше имя")]
        [StringLength(100, ErrorMessage = "Максимум 100 символов")]
        public string Name { get; set; } = "";

        [Display(Name = "Телефон")]
        [Required(ErrorMessage = "Введите номер телефона")]
        [Phone(ErrorMessage = "Введите корректный номер")]
        public string Phone { get; set; } = "";

        [Display(Name = "Полный адрес")]
        [Required(ErrorMessage = "Введите ваш адрес")]
        public string Address { get; set; } = "";

        [Display(Name = "E-mail")]
        [Required(ErrorMessage = "Введите email")]
        [EmailAddress(ErrorMessage = "Введите корректный email")]
        public string Email { get; set; } = "";

        [Display(Name = "Подтверждение E-mail")]
        [Required(ErrorMessage = "Подтвердите email")]
        [Compare("Email", ErrorMessage = "Email и подтверждение не совпадают")]
        public string EmailConfirm { get; set; } = "";

        [Display(Name = "Страна проживания")]
        [Required(ErrorMessage = "Выберите страну")]
        public string Country { get; set; } = "";

        [Display(Name = "Согласие с условиями")]
        [Required(ErrorMessage = "Вы должны принять условия")]
        public string Agree { get; set; } = ""; // строка, не bool!

        // Дата создания заявки
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Статус (для администратора)
        public string Status { get; set; } = "Новая";
    }
}
