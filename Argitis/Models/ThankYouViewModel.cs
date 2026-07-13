namespace Argitis.Models
{
    public class ThankYouViewModel
    {
        public string RequestId { get; set; } = $"REQ-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        public string ClientName { get; set; } = "";
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "EUR";
        public int Period { get; set; }
        public decimal MonthlyPayment { get; set; }
        public decimal TotalAmount { get; set; }
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";

        // Для отображения
        public string FormattedAmount => $"{Amount:N0} {Currency}";
        public string FormattedMonthlyPayment => $"{MonthlyPayment:N2} {Currency}";
        public string FormattedTotalAmount => $"{TotalAmount:N2} {Currency}";
        public string FormattedPeriod => $"{Period} {(Period == 1 ? "Месяц" : Period < 5 ? "Месяца" : "Месяцев")}";
    }
}
