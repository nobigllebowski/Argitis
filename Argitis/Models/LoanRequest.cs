using System.ComponentModel.DataAnnotations;

namespace Argitis.Models
{
    public class LoanRequest
    {
        [Display(Name = "Desired amount")]
        [Required(ErrorMessage = "Please enter the desired amount.")]
        [Range(1000, 1000000, ErrorMessage = "The amount must be between 1000 and 1,000,000.")]
        public decimal Amount { get; set; }

        [Display(Name = "Currency")]
        [Required(ErrorMessage = "Please select a currency.")]
        public string Currency { get; set; } = "EUR";

        [Display(Name = "Term (months)")]
        [Required(ErrorMessage = "Please enter the loan term.")]
        [Range(1, 360, ErrorMessage = "The term must be between 1 and 360 months.")]
        public int Period { get; set; }

        [Display(Name = "First and last name")]
        [Required(ErrorMessage = "Please enter your name.")]
        [StringLength(100, ErrorMessage = "Maximum 100 characters allowed.")]
        public string Name { get; set; } = "";

        [Display(Name = "Phone")]
        [Required(ErrorMessage = "Please enter your phone number.")]
        [Phone(ErrorMessage = "Please enter a valid phone number.")]
        public string Phone { get; set; } = "";

        [Display(Name = "Full address")]
        [Required(ErrorMessage = "Please enter your address.")]
        public string Address { get; set; } = "";

        [Display(Name = "E-mail")]
        [Required(ErrorMessage = "Please enter your e-mail.")]
        [EmailAddress(ErrorMessage = "Please enter a valid e-mail address.")]
        public string Email { get; set; } = "";

        [Display(Name = "Confirm E-mail")]
        [Required(ErrorMessage = "Please confirm your e-mail.")]
        [Compare("Email", ErrorMessage = "The e-mail and confirmation do not match.")]
        public string EmailConfirm { get; set; } = "";

        [Display(Name = "Country of residence")]
        [Required(ErrorMessage = "Please select a country.")]
        public string Country { get; set; } = "";

        [Display(Name = "Agree to terms")]
        [Required(ErrorMessage = "You must accept the terms and conditions.")]
        public string Agree { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Новая";
    }
}