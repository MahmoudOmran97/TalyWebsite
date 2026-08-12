using TalyWebsite.Models;

namespace TalyWebsite.Services
{
    // بيقرأ الروابط مباشرة من appsettings.json (قسم SocialLinks) من غير أي اتصال بأي API خارجي.
    // علشان تغيّر أي لينك أو تضيف واحد جديد، افتح appsettings.json وعدّل قسم "SocialLinks" بس.
    public class SiteLinkService
    {
        private readonly IConfiguration _configuration;

        public SiteLinkService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<List<SiteLink>> GetActiveLinksAsync()
        {
            var links = _configuration.GetSection("SocialLinks").Get<List<SiteLink>>() ?? new List<SiteLink>();

            var result = links
                .Where(l => l.IsActive)
                .OrderBy(l => l.SortOrder)
                .ToList();

            return Task.FromResult(result);
        }
    }
}
