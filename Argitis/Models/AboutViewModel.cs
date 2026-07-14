namespace Argitis.Models
{
    public class AboutViewModel
    {
        public string Title { get; set; } = "About - VeltisGroup";
        public string MetaDescription { get; set; } = "We specialize in financial services for individuals and professionals.";
        public string MetaKeywords { get; set; } = "Finance, Loan, Personal loan, Home loan, Mortgage Loan, Auto Credit, Mortgage Credit, Education Financing, Loan information, Requested amount";

        // Данные для секций (будут заполняться через контроллер)
        public string Heading { get; set; } = "";
        public string Description { get; set; } = "";

        // Миссия и видение
        public string MissionTitle { get; set; } = "";
        public string MissionText { get; set; } = "";

        public string VisionTitle { get; set; } = "";
        public string VisionText { get; set; } = "";

        // Счетчики (числа не переводятся)
        public int ExperienceYears { get; set; } = 10;
        public int HappyClients { get; set; } = 5000;
        public int Reviews { get; set; } = 20;
        public int Awards { get; set; } = 0;

        // Отзывы
        public List<TestimonialItem> Testimonials { get; set; } = new List<TestimonialItem>();

        // FAQ
        public List<FaqItem> FaqItems { get; set; } = new List<FaqItem>();

        public class TestimonialItem
        {
            public string Quote { get; set; }
            public string Author { get; set; }
            public string Role { get; set; }
        }

        public class FaqItem
        {
            public string Question { get; set; }
            public string Answer { get; set; }
            public bool IsExpanded { get; set; } = false;
        }
    }
}