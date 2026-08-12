using System.Text.Json;
using TalyWebsite.Models;

namespace TalyWebsite.Services
{
    public class SiteLinkService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<SiteLinkService> _logger;
        private readonly IConfiguration _configuration;

        public SiteLinkService(HttpClient httpClient, ILogger<SiteLinkService> logger, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<List<SiteLink>> GetActiveLinksAsync()
        {
            List<SiteLink> links = new List<SiteLink>();
            var apiBaseUrl = _configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7001";

            try
            {
                var response = await _httpClient.GetAsync($"{apiBaseUrl}/api/sitelinks");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var apiLinks = JsonSerializer.Deserialize<List<SiteLink>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    if (apiLinks != null)
                    {
                        links = apiLinks.Where(l => l.IsActive).ToList();
                    }
                }
            }
            catch (Exception)
            {
                // تسجيل تحذير مبسط دون إزعاج المستخدم إذا كان الـ API غير متصل محلياً
                _logger.LogDebug($"API not reachable at {apiBaseUrl}. Using default fallback links.");
            }

            // روابط افتراضية مع الأيقونات المخصصة في حال عدم توفر الاتصال بالـ API
            if (links.Count == 0)
            {
                links = new List<SiteLink>
                {
                    new SiteLink { Key = "whatsapp", Title = "WhatsApp", Url = "https://wa.me/201000000000", Icon = "/images/icons/whatsapp.png", IsActive = true, SortOrder = 1 },
                    new SiteLink { Key = "facebook", Title = "Facebook", Url = "https://facebook.com", Icon = "/images/icons/facebook.png", IsActive = true, SortOrder = 2 },
                    new SiteLink { Key = "instagram", Title = "Instagram", Url = "https://instagram.com", Icon = "/images/icons/instagram.png", IsActive = true, SortOrder = 3 },
                    new SiteLink { Key = "tiktok", Title = "TikTok", Url = "https://tiktok.com", Icon = "/images/icons/tiktok.png", IsActive = true, SortOrder = 4 },
                    new SiteLink { Key = "x", Title = "X", Url = "https://x.com", Icon = "/images/icons/x.png", IsActive = true, SortOrder = 5 }
                };
            }
            else
            {
                foreach (var link in links)
                {
                    switch (link.Key.ToLower())
                    {
                        case "whatsapp": link.Icon = "/images/icons/whatsapp.png"; break;
                        case "facebook": link.Icon = "/images/icons/facebook.png"; break;
                        case "instagram": link.Icon = "/images/icons/instagram.png"; break;
                        case "tiktok": link.Icon = "/images/icons/tiktok.png"; break;
                        case "x": link.Icon = "/images/icons/x.png"; break;
                    }
                }
            }

            return links.OrderBy(l => l.SortOrder).ToList();
        }
    }
}
