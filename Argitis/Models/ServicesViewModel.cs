namespace Argitis.Models
{
    public class ServicesViewModel
    {
        public string Title { get; set; } = "Services - GROUPE-ARGITIS";
        public string MetaDescription { get; set; } = "We specialize in financial services for individuals and professionals.";
        public string MetaKeywords { get; set; } = "Finance, Loan, Personal loan, Home loan, Mortgage Loan, Auto Credit, Mortgage Credit, Education Financing, Loan information, Requested amount";

        public string PageTitle { get; set; } = "Наши услуги";
        public string PageHeading { get; set; } = "НАШИ РЕШЕНИЯ";
        public string PageDescription { get; set; } = "Индивидуальные решения для каждой потребности";

        public List<ServiceItem> Services { get; set; } = new List<ServiceItem>();

        public class ServiceItem
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string ImageUrl { get; set; }
            public string Link { get; set; }
        }
    }
}
