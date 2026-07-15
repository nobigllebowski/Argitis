namespace Argitis.Models
{
    public class HomeViewModel
    {
        public string Title { get; set; } = "Home - VELTISGROUP";
        public string MetaDescription { get; set; } = "We specialize in financial services for individuals and professionals.";
        public string MetaKeywords { get; set; } = "Finance, Loan, Personal loan, Home loan, Mortgage Loan, Auto Credit, Mortgage Credit, Education Financing, Loan information, Requested amount";

        // Данные для секций
        public List<ServiceItem> Services { get; set; } = new List<ServiceItem>();
        public List<TestimonialItem> Testimonials { get; set; } = new List<TestimonialItem>();
        public List<ProcessStep> ProcessSteps { get; set; } = new List<ProcessStep>();
        public List<FaqItem> FaqItems { get; set; } = new List<FaqItem>();

        // Данные для контактов
        public string Email { get; set; } = "help@veltisgroup.org";
        public string Phone { get; set; } = "+39 06XXXXXX26";
        public string Address { get; set; } = "Welserstr. 13-15 (Geisbeegerstr.), 10777 Berlin";

        // Счетчики (для odometer)
        public int ExperienceYears { get; set; } = 10;
        public int HappyClients { get; set; } = 5000;
        public int Reviews { get; set; } = 20;
        public int Awards { get; set; } = 0;

        public class ServiceItem
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string ImageUrl { get; set; }
            public string Link { get; set; }
        }

        public class TestimonialItem
        {
            public string Quote { get; set; }
            public string Author { get; set; }
            public string Role { get; set; }
            public string ImageUrl { get; set; }
        }

        public class ProcessStep
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public string IconClass { get; set; }
            public int StepNumber { get; set; }
        }

        public class FaqItem
        {
            public string Question { get; set; }
            public string Answer { get; set; }
            public bool IsExpanded { get; set; } = false;
        }
    }
}
