using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using NuGet.Protocol.Plugins;
using Pomodoro.Library.Models.Identity;
using TimeZoneConverter;

namespace Pomodoro.UI.Util.TagHelpers;

[HtmlTargetElement("local-time")]
public class LocalTimeTagHelper : TagHelper
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LocalTimeTagHelper(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    [HtmlAttributeName("utc")]
    public DateTime UtcDateTime { get; set; }
    public string Format { get; set; } = String.Empty;

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

        var ianaTimeZone = user?.IanaTimeZone;

        if (ianaTimeZone != null)
        {
            var timeZoneInfo = TZConvert.GetTimeZoneInfo(ianaTimeZone);
            var localDateTime = TimeZoneInfo.ConvertTimeFromUtc(UtcDateTime, timeZoneInfo);

            output.Content.SetContent(localDateTime.ToString(Format)); // "f" for full date and time pattern
        }
        else
        {
            // Fallback: If the timezone is not available, just render the UTC DateTime
            output.Content.SetContent(UtcDateTime.ToString());
        }
    }
}

