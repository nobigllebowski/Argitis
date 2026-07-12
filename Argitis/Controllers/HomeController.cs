using Argitis.Models;
using Microsoft.AspNetCore.Mvc;

namespace Argitis.Controllers
{
    public class HomeController : Controller
    {
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

        [HttpGet]
        public IActionResult Appointment()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Appointment(string name, string phone, string email, int amount, int period)
        {
            // Здесь логика сохранения заявки
            // Например: _dbContext.LoanRequests.Add(...);
            // _dbContext.SaveChanges();

            TempData["Success"] = "Ваша заявка успешно отправлена! Мы свяжемся с вами в ближайшее время.";
            return RedirectToAction("Index");
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Services()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
    }
}
