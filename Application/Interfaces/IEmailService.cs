using Application.DTOs;

namespace CreditPortal.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailDto email);
        string BuildLoanConfirmationEmail(LoanConfirmationDto loan, string companyName, string companyEmail, string language);
        string BuildAdminNotificationEmail(LoanConfirmationDto loan, string clientIp);
    }
}
