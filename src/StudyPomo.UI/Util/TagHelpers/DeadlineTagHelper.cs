using Humanizer;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace StudyPomo.UI.Util.TagHelpers;

public class DeadlineTagHelper : TagHelper
{
    [HtmlAttributeName("display-tz")]
    public string DisplayTimeZone { get; set; }

    public DateTime? Date { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "span";

        string textColor = string.Empty;

        if (Date.HasValue)
        {
            TimeSpan timeToDeadline = Date.Value - DateTime.UtcNow;

            if (timeToDeadline.TotalHours < 0)
            {
                textColor = "overdue-color";
            }
            else if (timeToDeadline.TotalDays < 1)
            {
                textColor = "almost-due-color";
            }
            else
            {
                textColor = "text-muted";
            }
        }

        output.Attributes.SetAttribute("class", textColor);

        if (Date.HasValue && DisplayTimeZone is not null)
        {
            var timezone = TimeZoneInfo.FindSystemTimeZoneById(DisplayTimeZone);

            if (timezone is not null)
            {
                var localTime = TimeZoneInfo.ConvertTimeFromUtc(Date.Value, timezone);

                output.Attributes.SetAttribute("data-bs-toggle", "popover");
                output.Attributes.SetAttribute("data-bs-trigger", "hover focus");
                output.Attributes.SetAttribute("data-bs-placement", "top");
                output.Attributes.SetAttribute("data-bs-content", localTime.ToString());
                output.Attributes.SetAttribute("data-bs-delay", "{ \"show\": 300, \"hide\": 100 }"); // Delayed popover
            }

        }

        // Modify the text if priority is set
        string displayText = Date.Humanize();
        output.Content.SetContent(displayText);
    }
}
