namespace Argitis.Models
{
    public class AboutViewModel
    {
        public string Title { get; set; } = "About - GROUPE-ARGITIS";
        public string MetaDescription { get; set; } = "We specialize in financial services for individuals and professionals.";
        public string MetaKeywords { get; set; } = "Finance, Loan, Personal loan, Home loan, Mortgage Loan, Auto Credit, Mortgage Credit, Education Financing, Loan information, Requested amount";

        // Данные для секций
        public string Heading { get; set; } = "Ваш надежный партнер";
        public string Description { get; set; } = "Мы специализируемся на финансовых услугах для частных лиц и профессионалов. Наш опыт и надежность делают нас надежным партнером.";

        // Миссия и видение
        public string MissionTitle { get; set; } = "Наша миссия";
        public string MissionText { get; set; } = "Помогать нашим клиентам достигать их финансовых целей, предлагая гибкие и конкурентоспособные финансовые решения.";

        public string VisionTitle { get; set; } = "Наше видение";
        public string VisionText { get; set; } = "Быть надежным партнером, который помогает своим клиентам развиваться, предлагая качественные финансовые услуги.";

        // Счетчики
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
