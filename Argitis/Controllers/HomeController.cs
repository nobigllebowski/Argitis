using Argitis.DTOs;
using Argitis.Models;
using Argitis.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Argitis.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly IStringLocalizer<Resources.Controllers.HomeController> _localizer;

        public HomeController(IEmailService emailService, IStringLocalizer<Resources.Controllers.HomeController> localizer)
        {
            _emailService = emailService;
            _localizer = localizer;
        }

        public IActionResult Index()
        {
            var model = new HomeViewModel();

            model.Services = new List<HomeViewModel.ServiceItem>
            {
                new HomeViewModel.ServiceItem { Name = _localizer["PersonalLoan"], Description = _localizer["PersonalLoanDesc"], ImageUrl = "~/images/personnel.webp", Link = "/Home/Appointment" },
                new HomeViewModel.ServiceItem { Name = _localizer["CarLoan"], Description = _localizer["CarLoanDesc"], ImageUrl = "~/images/automobile.webp", Link = "/Home/Appointment" },
                new HomeViewModel.ServiceItem { Name = _localizer["MortgageLoan"], Description = _localizer["MortgageLoanDesc"], ImageUrl = "~/images/immo.jpg", Link = "/Home/Appointment" },
                new HomeViewModel.ServiceItem { Name = _localizer["InvestmentLoan"], Description = _localizer["InvestmentLoanDesc"], ImageUrl = "~/images/inv.jpg", Link = "/Home/Appointment" },
                new HomeViewModel.ServiceItem { Name = _localizer["DebtConsolidation"], Description = _localizer["DebtConsolidationDesc"], ImageUrl = "~/images/debt.webp", Link = "/Home/Appointment" },
                new HomeViewModel.ServiceItem { Name = _localizer["Refinancing"], Description = _localizer["RefinancingDesc"], ImageUrl = "~/images/regroup.webp", Link = "/Home/Appointment" }
            };

            model.Testimonials = new List<HomeViewModel.TestimonialItem>
            {
                new HomeViewModel.TestimonialItem { Quote = _localizer["Testimonial1Quote"], Author = _localizer["Testimonial1Author"], Role = _localizer["Testimonial1Role"], ImageUrl = "https://randomuser.me/api/portraits/women/1.jpg" },
                new HomeViewModel.TestimonialItem { Quote = _localizer["Testimonial2Quote"], Author = _localizer["Testimonial2Author"], Role = _localizer["Testimonial2Role"], ImageUrl = "https://randomuser.me/api/portraits/men/2.jpg" },
                new HomeViewModel.TestimonialItem { Quote = _localizer["Testimonial3Quote"], Author = _localizer["Testimonial3Author"], Role = _localizer["Testimonial3Role"], ImageUrl = "https://randomuser.me/api/portraits/men/3.jpg" },
                new HomeViewModel.TestimonialItem { Quote = _localizer["Testimonial4Quote"], Author = _localizer["Testimonial4Author"], Role = _localizer["Testimonial4Role"], ImageUrl = "https://randomuser.me/api/portraits/men/4.jpg" },
                new HomeViewModel.TestimonialItem { Quote = _localizer["Testimonial5Quote"], Author = _localizer["Testimonial5Author"], Role = _localizer["Testimonial5Role"], ImageUrl = "https://randomuser.me/api/portraits/women/5.jpg" },
                new HomeViewModel.TestimonialItem { Quote = _localizer["Testimonial6Quote"], Author = _localizer["Testimonial6Author"], Role = _localizer["Testimonial6Role"], ImageUrl = "https://randomuser.me/api/portraits/men/6.jpg" }
            };

            model.ProcessSteps = new List<HomeViewModel.ProcessStep>
            {
                new HomeViewModel.ProcessStep { Title = _localizer["Step1Title"], Description = _localizer["Step1Desc"], IconClass = "icon-AddressBook", StepNumber = 1 },
                new HomeViewModel.ProcessStep { Title = _localizer["Step2Title"], Description = _localizer["Step2Desc"], IconClass = "icon-ListChecks", StepNumber = 2 },
                new HomeViewModel.ProcessStep { Title = _localizer["Step3Title"], Description = _localizer["Step3Desc"], IconClass = "icon-FlowerLotus", StepNumber = 3 },
                new HomeViewModel.ProcessStep { Title = _localizer["Step4Title"], Description = _localizer["Step4Desc"], IconClass = "icon-Lifebuoy", StepNumber = 4 }
            };

            model.FaqItems = new List<HomeViewModel.FaqItem>
            {
                new HomeViewModel.FaqItem { Question = _localizer["Faq1Q"], Answer = _localizer["Faq1A"], IsExpanded = false },
                new HomeViewModel.FaqItem { Question = _localizer["Faq2Q"], Answer = _localizer["Faq2A"], IsExpanded = true },
                new HomeViewModel.FaqItem { Question = _localizer["Faq3Q"], Answer = _localizer["Faq3A"], IsExpanded = false },
                new HomeViewModel.FaqItem { Question = _localizer["Faq4Q"], Answer = _localizer["Faq4A"], IsExpanded = false }
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Services()
        {
            var model = new ServicesViewModel();

            model.Services = new List<ServicesViewModel.ServiceItem>
            {
                new ServicesViewModel.ServiceItem { Name = _localizer["PersonalLoan"], Description = _localizer["PersonalLoanDesc"], ImageUrl = "~/images/personnel.webp", Link = "/Home/Appointment" },
                new ServicesViewModel.ServiceItem { Name = _localizer["CarLoan"], Description = _localizer["CarLoanDesc"], ImageUrl = "~/images/automobile.webp", Link = "/Home/Appointment" },
                new ServicesViewModel.ServiceItem { Name = _localizer["MortgageLoan"], Description = _localizer["MortgageLoanDesc"], ImageUrl = "~/images/immo.jpg", Link = "/Home/Appointment" },
                new ServicesViewModel.ServiceItem { Name = _localizer["InvestmentLoan"], Description = _localizer["InvestmentLoanDesc"], ImageUrl = "~/images/inv.jpg", Link = "/Home/Appointment" },
                new ServicesViewModel.ServiceItem { Name = _localizer["DebtConsolidation"], Description = _localizer["DebtConsolidationDesc"], ImageUrl = "~/images/debt.webp", Link = "/Home/Appointment" },
                new ServicesViewModel.ServiceItem { Name = _localizer["Refinancing"], Description = _localizer["RefinancingDesc"], ImageUrl = "~/images/regroup.webp", Link = "/Home/Appointment" }
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult About()
        {
            var model = new AboutViewModel();

            model.Heading = _localizer["AboutHeading"];
            model.Description = _localizer["AboutDescription"];
            model.MissionTitle = _localizer["MissionTitle"];
            model.MissionText = _localizer["MissionText"];
            model.VisionTitle = _localizer["VisionTitle"];
            model.VisionText = _localizer["VisionText"];

            model.Testimonials = new List<AboutViewModel.TestimonialItem>
            {
                new AboutViewModel.TestimonialItem { Quote = _localizer["Testimonial1Quote"], Author = _localizer["Testimonial1Author"], Role = _localizer["Testimonial1Role"] },
                new AboutViewModel.TestimonialItem { Quote = _localizer["Testimonial2Quote"], Author = _localizer["Testimonial2Author"], Role = _localizer["Testimonial2Role"] },
                new AboutViewModel.TestimonialItem { Quote = _localizer["Testimonial3Quote"], Author = _localizer["Testimonial3Author"], Role = _localizer["Testimonial3Role"] },
                new AboutViewModel.TestimonialItem { Quote = _localizer["Testimonial4Quote"], Author = _localizer["Testimonial4Author"], Role = _localizer["Testimonial4Role"] }
            };

            model.FaqItems = new List<AboutViewModel.FaqItem>
            {
                new AboutViewModel.FaqItem { Question = _localizer["Faq1Q"], Answer = _localizer["Faq1A"], IsExpanded = false },
                new AboutViewModel.FaqItem { Question = _localizer["Faq2Q"], Answer = _localizer["Faq2A"], IsExpanded = true },
                new AboutViewModel.FaqItem { Question = _localizer["Faq3Q"], Answer = _localizer["Faq3A"], IsExpanded = false },
                new AboutViewModel.FaqItem { Question = _localizer["Faq4Q"], Answer = _localizer["Faq4A"], IsExpanded = false }
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Contact()
        {
            var model = new ContactViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(ContactFormModel model)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new ContactViewModel();
                return View(viewModel);
            }

            try
            {
                var adminEmail = new EmailDto
                {
                    ToEmail = "help@veltisgroup.org",
                    ToName = "Administrator",
                    Subject = "📝 Nouveau message depuis le formulaire de contact",
                    Body = $@"
                        <h2>Nouveau message depuis le formulaire de contact</h2>
                        <p><strong>Nom :</strong> {model.Name}</p>
                        <p><strong>E-mail :</strong> {model.Email}</p>
                        <p><strong>Téléphone :</strong> {model.Phone}</p>
                        <p><strong>Message :</strong></p>
                        <p>{model.Message}</p>
                        <p><strong>Date :</strong> {DateTime.Now:dd/MM/yyyy HH:mm}</p>
                    "
                };
                await _emailService.SendEmailAsync(adminEmail);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка отправки email: {ex.Message}");
            }

            TempData["SuccessMessageKey"] = "ContactSuccessMessage";
            return RedirectToAction("Contact");
        }

        [HttpGet]
        public IActionResult Appointment()
        {
            var model = new LoanRequest();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Appointment(LoanRequest model)
        {
            if (model.Agree != "true" && model.Agree != "on")
            {
                ModelState.AddModelError("Agree", "Вы должны принять условия");
                return View(model);
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            decimal monthlyPayment = CalculateMonthlyPayment(model.Amount, 2.5m, model.Period);
            decimal totalAmount = monthlyPayment * model.Period;

            string requestId = $"REQ-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";

            var loanDto = new LoanConfirmationDto
            {
                RequestId = requestId,
                FullName = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                Address = model.Address,
                Country = model.Country,
                Amount = model.Amount,
                Currency = model.Currency,
                Months = model.Period,
                InterestRate = 2.5m,
                MonthlyPayment = monthlyPayment,
                TotalAmount = totalAmount,
                Language = _localizer["LanguageCode"] // <--- Язык клиента (динамически из ресурсов)
            };

            var thankYouModel = new ThankYouViewModel
            {
                RequestId = requestId,
                ClientName = model.Name,
                Amount = model.Amount,
                Currency = model.Currency,
                Period = model.Period,
                MonthlyPayment = monthlyPayment,
                TotalAmount = totalAmount,
                Email = model.Email,
                Phone = model.Phone
            };

            try
            {
                var clientEmail = new EmailDto
                {
                    ToEmail = model.Email,
                    ToName = model.Name,
                    Subject = _localizer["ClientEmailSubject"], // <--- Локализованная тема для клиента
                    Body = _emailService.BuildLoanConfirmationEmail(
                        loanDto,
                        "VeltisGroup",
                        "contact@veltisgroup.com",
                        loanDto.Language
                    )
                };
                await _emailService.SendEmailAsync(clientEmail);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка отправки email клиенту: {ex.Message}");
            }

            try
            {
                var adminEmail = new EmailDto
                {
                    ToEmail = "help@veltisgroup.org",
                    ToName = "Administrator",
                    Subject = $"📝 Nouvelle demande de prêt - {model.Name}",
                    Body = _emailService.BuildAdminNotificationEmail(
                        loanDto,
                        HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown"
                    )
                };
                await _emailService.SendEmailAsync(adminEmail);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка отправки email администратору: {ex.Message}");
            }

            TempData["SuccessMessageKey"] = "AppointmentSuccessMessage";
            return RedirectToAction("AppointmentSuccess", thankYouModel);
        }

        [HttpGet]
        public IActionResult AppointmentSuccess(ThankYouViewModel model)
        {
            if (string.IsNullOrEmpty(model.ClientName))
            {
                model.ClientName = "Клиент";
                model.Amount = 10000;
                model.Currency = "EUR";
                model.Period = 12;
                model.MonthlyPayment = 844.66m;
                model.TotalAmount = 10135.93m;
                model.RequestId = $"REQ-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
            }
            return View(model);
        }

        private decimal CalculateMonthlyPayment(decimal principal, decimal annualRate, int months)
        {
            decimal monthlyRate = annualRate / 12 / 100;
            if (monthlyRate == 0) return principal / months;

            decimal factor = (decimal)Math.Pow((double)(1 + monthlyRate), months);
            return principal * monthlyRate * factor / (factor - 1);
        }
    }
}