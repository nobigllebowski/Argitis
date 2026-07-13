using Argitis.DTOs;
using Argitis.Models;
using Argitis.Services;
using Microsoft.AspNetCore.Mvc;

namespace Argitis.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmailService _emailService;

        // Внедряем IEmailService через конструктор
        public HomeController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            var model = new HomeViewModel();

            // Услуги
            model.Services = new List<HomeViewModel.ServiceItem>
            {
                new HomeViewModel.ServiceItem
                {
                    Name = "Персональный кредит",
                    Description = "Наши персональные кредиты помогают вам достичь ваших финансовых целей по конкурентоспособным процентным ставкам.",
                    ImageUrl = "~/images/personnel.webp",
                    Link = "/Home/Appointment"
                },
                new HomeViewModel.ServiceItem
                {
                    Name = "Автокредит",
                    Description = "Автокредит - это решение для покупки нового или подержанного автомобиля. Мы предлагаем гибкие условия.",
                    ImageUrl = "~/images/automobile.webp",
                    Link = "/Home/Appointment"
                },
                new HomeViewModel.ServiceItem
                {
                    Name = "Ипотечный кредит",
                    Description = "Купите дом своей мечты благодаря нашему индивидуальному и безопасному решению ипотечного кредита.",
                    ImageUrl = "~/images/immo.jpg",
                    Link = "/Home/Appointment"
                },
                new HomeViewModel.ServiceItem
                {
                    Name = "Инвестиционный кредит",
                    Description = "Инвестиционный кредит подходит для клиентов, которые хотят развивать свои инвестиции или запускать новые проекты.",
                    ImageUrl = "~/images/inv.jpg",
                    Link = "/Home/Appointment"
                },
                new HomeViewModel.ServiceItem
                {
                    Name = "Выкуп/реструктуризация долгов",
                    Description = "Объедините ваши кредиты и воспользуйтесь более выгодными условиями для оптимизации вашего бюджета.",
                    ImageUrl = "~/images/debt.webp",
                    Link = "/Home/Appointment"
                },
                new HomeViewModel.ServiceItem
                {
                    Name = "Рефинансирование кредитов",
                    Description = "Наши персональные кредиты помогают вам достичь ваших финансовых целей по конкурентоспособным процентным ставкам.",
                    ImageUrl = "~/images/regroup.webp",
                    Link = "/Home/Appointment"
                }
            };

            // Отзывы
            model.Testimonials = new List<HomeViewModel.TestimonialItem>
            {
                new HomeViewModel.TestimonialItem
                {
                    Quote = "Благодаря GROUPE-ARGITIS я смог финансировать свой новый автомобиль без стресса. Команда была очень профессиональной и внимательной.",
                    Author = "Г-жа Дюпон",
                    Role = "Учитель"
                },
                new HomeViewModel.TestimonialItem
                {
                    Quote = "Я объединил свои кредиты, и теперь у меня есть ежемесячный платеж, соответствующий моему бюджету. Спасибо всей команде за их поддержку!",
                    Author = "Г-н Мартен",
                    Role = "Бухгалтер"
                },
                new HomeViewModel.TestimonialItem
                {
                    Quote = "Обслуживание клиентов было отличным. Персонал был дружелюбным и ответил на все мои вопросы.",
                    Author = "Г-н Бернар",
                    Role = "Водитель"
                },
                new HomeViewModel.TestimonialItem
                {
                    Quote = "Мне понравилась прозрачность и скорость обслуживания. GROUPE-ARGITIS поддерживала меня на каждом этапе моей заявки на кредит.",
                    Author = "Michael L.",
                    Role = "Торговый представитель"
                }
            };

            // Процесс (4 шага)
            model.ProcessSteps = new List<HomeViewModel.ProcessStep>
            {
                new HomeViewModel.ProcessStep
                {
                    Title = "Подайте заявку",
                    Description = "Заполните форму заявки онлайн. Просто, быстро и безопасно.",
                    IconClass = "icon-AddressBook",
                    StepNumber = 1
                },
                new HomeViewModel.ProcessStep
                {
                    Title = "Мы изучаем вашу заявку",
                    Description = "Мы изучаем ваше дело бесплатно и информируем вас о результате.",
                    IconClass = "icon-ListChecks",
                    StepNumber = 2
                },
                new HomeViewModel.ProcessStep
                {
                    Title = "Обработка заявки",
                    Description = "Мы свяжемся с вами для получения дополнительной информации и оценки вашей заявки.",
                    IconClass = "icon-FlowerLotus",
                    StepNumber = 3
                },
                new HomeViewModel.ProcessStep
                {
                    Title = "Получите деньги",
                    Description = "После одобрения деньги будут переведены непосредственно на ваш счет.",
                    IconClass = "icon-Lifebuoy",
                    StepNumber = 4
                }
            };

            // FAQ
            model.FaqItems = new List<HomeViewModel.FaqItem>
            {
                new HomeViewModel.FaqItem
                {
                    Question = "Персональный кредит",
                    Answer = "Наши персональные кредиты помогают вам достичь ваших финансовых целей по конкурентоспособным процентным ставкам.",
                    IsExpanded = false
                },
                new HomeViewModel.FaqItem
                {
                    Question = "Автокредит",
                    Answer = "Автокредит - это решение для покупки нового или подержанного автомобиля. Мы предлагаем гибкие условия.",
                    IsExpanded = true
                },
                new HomeViewModel.FaqItem
                {
                    Question = "Ипотечный кредит",
                    Answer = "Наши ипотечные кредиты помогают вам осуществить вашу мечту о собственности. Зачем арендовать, когда можно купить?",
                    IsExpanded = false
                },
                new HomeViewModel.FaqItem
                {
                    Question = "Финансирование обучения",
                    Answer = "",
                    IsExpanded = false
                }
            };

            return View(model);
        }

        // GET: /Home/Services
        [HttpGet]
        public IActionResult Services()
        {
            var model = new ServicesViewModel();

            // Заполняем услуги
            model.Services = new List<ServicesViewModel.ServiceItem>
    {
        new ServicesViewModel.ServiceItem
        {
            Name = "Персональный кредит",
            Description = "Наши персональные кредиты помогают вам достичь ваших финансовых целей по конкурентоспособным процентным ставкам.",
            ImageUrl = "~/images/personnel.webp",
            Link = "/Home/Appointment"
        },
        new ServicesViewModel.ServiceItem
        {
            Name = "Автокредит",
            Description = "Автокредит - это решение для покупки нового или подержанного автомобиля. Мы предлагаем гибкие условия.",
            ImageUrl = "~/images/automobile.webp",
            Link = "/Home/Appointment"
        },
        new ServicesViewModel.ServiceItem
        {
            Name = "Ипотечный кредит",
            Description = "Купите дом своей мечты благодаря нашему индивидуальному и безопасному решению ипотечного кредита.",
            ImageUrl = "~/images/immo.jpg",
            Link = "/Home/Appointment"
        },
        new ServicesViewModel.ServiceItem
        {
            Name = "Инвестиционный кредит",
            Description = "Инвестиционный кредит подходит для клиентов, которые хотят развивать свои инвестиции или запускать новые проекты.",
            ImageUrl = "~/images/inv.jpg",
            Link = "/Home/Appointment"
        },
        new ServicesViewModel.ServiceItem
        {
            Name = "Выкуп/реструктуризация долгов",
            Description = "Объедините ваши кредиты и воспользуйтесь более выгодными условиями для оптимизации вашего бюджета.",
            ImageUrl = "~/images/debt.webp",
            Link = "/Home/Appointment"
        },
        new ServicesViewModel.ServiceItem
        {
            Name = "Рефинансирование кредитов",
            Description = "Наши персональные кредиты помогают вам достичь ваших финансовых целей по конкурентоспособным процентным ставкам.",
            ImageUrl = "~/images/regroup.webp",
            Link = "/Home/Appointment"
        }
    };

            return View(model);
        }

        [HttpGet]
        public IActionResult Contact()
        {
            var model = new ContactViewModel();
            return View(model);
        }

        // POST: /Home/Contact (обработка формы)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(ContactFormModel model)
        {
            if (!ModelState.IsValid)
            {
                // Возвращаем ту же страницу с ошибками
                var viewModel = new ContactViewModel();
                return View(viewModel);
            }

            // Сохранить в БД
            // _context.ContactMessages.Add(model);
            // await _context.SaveChangesAsync();

            // Отправить email администратору
            try
            {
                var adminEmail = new EmailDto
                {
                    ToEmail = "admin@groupeargitis.com",
                    ToName = "Administrator",
                    Subject = $"📝 Новое сообщение от {model.Name}",
                    Body = $@"
                <h2>Новое сообщение из формы контакта</h2>
                <p><strong>Имя:</strong> {model.Name}</p>
                <p><strong>Email:</strong> {model.Email}</p>
                <p><strong>Телефон:</strong> {model.Phone}</p>
                <p><strong>Сообщение:</strong></p>
                <p>{model.Message}</p>
                <p><strong>Дата:</strong> {DateTime.Now:dd.MM.yyyy HH:mm}</p>
            "
                };
                await _emailService.SendEmailAsync(adminEmail);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка отправки email: {ex.Message}");
            }

            TempData["SuccessMessage"] = "Ваше сообщение успешно отправлено! Мы свяжемся с вами в ближайшее время.";

            return RedirectToAction("Contact");
        }

        // GET: /Home/Appointment
        [HttpGet]
        public IActionResult Appointment()
        {
            var model = new LoanRequest();
            return View(model);
        }

        // POST: /Home/Appointment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Appointment(LoanRequest model)
        {
            // Ручная проверка чекбокса
            if (model.Agree != "true" && model.Agree != "on")
            {
                ModelState.AddModelError("Agree", "Вы должны принять условия");
                return View(model);
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Сохранить в БД
            // _context.LoanRequests.Add(model);
            // await _context.SaveChangesAsync();

            // Рассчитываем ежемесячный платёж
            decimal monthlyPayment = CalculateMonthlyPayment(model.Amount, 2.5m, model.Period);
            decimal totalAmount = monthlyPayment * model.Period;

            // Генерируем ID заявки
            string requestId = $"REQ-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";

            // Создаём LoanConfirmationDto для email
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
                Language = "en" // можно заменить на model.Language, если добавите в модель
            };

            // Создаём модель для страницы благодарности
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

            // Отправляем email клиенту
            try
            {
                var clientEmail = new EmailDto
                {
                    ToEmail = model.Email,
                    ToName = model.Name,
                    Subject = "✅ Your loan application has been received - GROUPE-ARGITIS",
                    Body = _emailService.BuildLoanConfirmationEmail(
                        loanDto,
                        "GROUPE-ARGITIS",
                        "fafimi8425@acoxs.com",
                        "en" // язык
                    )
                };
                await _emailService.SendEmailAsync(clientEmail);
            }
            catch (Exception ex)
            {
                // Логировать ошибку, но не прерывать процесс
                Console.WriteLine($"Ошибка отправки email клиенту: {ex.Message}");
            }

            // Отправляем email администратору (опционально)
            try
            {
                var adminEmail = new EmailDto
                {
                    ToEmail = "fafimi8425@acoxs.com",
                    ToName = "Administrator",
                    Subject = $"📝 New loan application - {model.Name}",
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

            TempData["SuccessMessage"] = "Ваша заявка успешно отправлена!";

            return RedirectToAction("AppointmentSuccess", thankYouModel);
        }

        // Вспомогательный метод для расчёта ежемесячного платежа
        private decimal CalculateMonthlyPayment(decimal principal, decimal annualRate, int months)
        {
            decimal monthlyRate = annualRate / 12 / 100;
            if (monthlyRate == 0) return principal / months;

            decimal factor = (decimal)Math.Pow((double)(1 + monthlyRate), months);
            return principal * monthlyRate * factor / (factor - 1);
        }

        [HttpGet]
        public IActionResult AppointmentSuccess(ThankYouViewModel model)
        {
            // Если модель пустая (например, прямой переход на страницу), создаём заглушку
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

        // GET: /Home/About
        [HttpGet]
        public IActionResult About()
        {
            var model = new AboutViewModel();

            // Заполняем отзывы
            model.Testimonials = new List<AboutViewModel.TestimonialItem>
    {
        new AboutViewModel.TestimonialItem
        {
            Quote = "Благодаря GROUPE-ARGITIS я смог финансировать свой новый автомобиль без стресса. Команда была очень профессиональной и внимательной.",
            Author = "Г-жа Дюпон",
            Role = "Учитель"
        },
        new AboutViewModel.TestimonialItem
        {
            Quote = "Я объединил свои кредиты, и теперь у меня есть ежемесячный платеж, соответствующий моему бюджету. Спасибо всей команде за их поддержку!",
            Author = "Г-н Мартен",
            Role = "Бухгалтер"
        },
        new AboutViewModel.TestimonialItem
        {
            Quote = "Обслуживание клиентов было отличным. Персонал был дружелюбным и ответил на все мои вопросы.",
            Author = "Г-н Бернар",
            Role = "Водитель"
        },
        new AboutViewModel.TestimonialItem
        {
            Quote = "Мне понравилась прозрачность и скорость обслуживания. GROUPE-ARGITIS поддерживала меня на каждом этапе моей заявки на кредит.",
            Author = "Michael L.",
            Role = "Торговый представитель"
        }
    };

            // Заполняем FAQ
            model.FaqItems = new List<AboutViewModel.FaqItem>
    {
        new AboutViewModel.FaqItem
        {
            Question = "Персональный кредит",
            Answer = "Наши персональные кредиты помогают вам достичь ваших финансовых целей по конкурентоспособным процентным ставкам.",
            IsExpanded = false
        },
        new AboutViewModel.FaqItem
        {
            Question = "Автокредит",
            Answer = "Автокредит - это решение для покупки нового или подержанного автомобиля. Мы предлагаем гибкие условия.",
            IsExpanded = true
        },
        new AboutViewModel.FaqItem
        {
            Question = "Ипотечный кредит",
            Answer = "Наши ипотечные кредиты помогают вам осуществить вашу мечту о собственности. Зачем арендовать, когда можно купить?",
            IsExpanded = false
        },
        new AboutViewModel.FaqItem
        {
            Question = "Финансирование обучения",
            Answer = "",
            IsExpanded = false
        }
    };

            return View(model);
        }
    }
}
