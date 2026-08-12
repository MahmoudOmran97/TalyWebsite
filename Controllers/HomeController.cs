using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using TalyWebsite.Services;

namespace TalyWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly SiteLinkService _siteLinkService;

        public HomeController(SiteLinkService siteLinkService)
        {
            _siteLinkService = siteLinkService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.SocialLinks = await _siteLinkService.GetActiveLinksAsync();
            return View();
        }

        public IActionResult Privacy() => View();

        public IActionResult Terms() => View();

        public IActionResult Partner() => View();

        public IActionResult Rider() => View();

        [HttpGet]
        public IActionResult SetLanguage(string culture, string? returnUrl = null)
        {
            var supportedCultures = new[] { "ar", "en" };
            if (!supportedCultures.Contains(culture, StringComparer.OrdinalIgnoreCase))
            {
                culture = "ar";
            }

            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1),
                    IsEssential = true,
                    HttpOnly = false,
                    SameSite = SameSiteMode.Lax
                });

            return LocalRedirect(returnUrl ?? Url.Action(nameof(Index)) ?? "/");
        }
    }
}
