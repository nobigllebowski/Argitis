using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Argitis.Controllers
{
    public class LanguageController : Controller
    {
        [HttpPost]
        public IActionResult ChangeLanguage(string lang, string returnUrl)
        {
            // Список разрешенных языков (без ru и uk)
            var supportedCultures = new[] { "en", "fr", "de", "es", "fi", "hr", "it", "lt", "lv", "mt", "nl", "pl", "pt", "ro", "sk" };

            if (!supportedCultures.Contains(lang))
            {
                lang = "en"; // fallback
            }

            // Сохраняем язык в Cookie
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(lang)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddDays(30) }
            );

            // Также сохраняем в отдельную cookie для флага
            Response.Cookies.Append("lang", lang, new CookieOptions { Expires = DateTimeOffset.UtcNow.AddDays(30) });

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}