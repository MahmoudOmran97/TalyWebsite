using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using TalyWebsite.Models;
using TalyWebsite.Services;

namespace TalyWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly SiteLinkService _siteLinkService;

        // بيانات صفحات الخدمات (مطاعم / صيدليات / سوبر ماركت / إكسسوارات).
        // عايز تضيف خدمة جديدة أو تعدّل واحدة موجودة؟ زوّد أو عدّل هنا بس، والصفحة هتتبني تلقائي.
        private static readonly Dictionary<string, ServiceInfo> Services = new(StringComparer.OrdinalIgnoreCase)
        {
            ["restaurants"] = new ServiceInfo
            {
                Key = "restaurants",
                Icon = "🍽️",
                TitleResourceKey = "Restaurants",
                DescriptionResourceKey = "RestaurantsDescription",
                BannerImage = "banner_general.png"
            },
            ["pharmacies"] = new ServiceInfo
            {
                Key = "pharmacies",
                Icon = "💊",
                TitleResourceKey = "Pharmacies",
                DescriptionResourceKey = "PharmaciesDescription",
                BannerImage = "banner_pharmacy.png"
            },
            ["supermarkets"] = new ServiceInfo
            {
                Key = "supermarkets",
                Icon = "🛒",
                TitleResourceKey = "Supermarkets",
                DescriptionResourceKey = "SupermarketsDescription",
                BannerImage = "banner_supermarket.png"
            },
            ["accessories"] = new ServiceInfo
            {
                Key = "accessories",
                Icon = "👜",
                TitleResourceKey = "Accessories",
                DescriptionResourceKey = "AccessoriesDescription",
                BannerImage = "banner_accessories.png"
            },
        };

        public HomeController(SiteLinkService siteLinkService)
        {
            _siteLinkService = siteLinkService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.SocialLinks = await _siteLinkService.GetActiveLinksAsync();
            return View();
        }

        public IActionResult Service(string id)
        {
            if (string.IsNullOrWhiteSpace(id) || !Services.TryGetValue(id, out var service))
            {
                return NotFound();
            }

            return View(service);
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
