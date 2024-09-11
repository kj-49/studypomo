using Humanizer;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Pomodoro.UI.Util.TagHelpers;

public class DeadlineTagHelper : TagHelper
{
    public DateTime? Date { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "span";

        string textColor = string.Empty;

        if (Date.HasValue)
        {
            TimeSpan timeToDeadline = Date.Value - DateTime.UtcNow;

            if (timeToDeadline.TotalHours < 6)
            {
                textColor = "text-danger fw-semibold"; // Less than 6 hours
            }
            else if (timeToDeadline.TotalDays < 3)
            {
                textColor = "text-warning fw-semibold"; // Less than 3 days
            }
            else
            {
                textColor = "text-muted"; // Default color or any other condition
            }
        }



        output.Attributes.SetAttribute("class", textColor);

        // Modify the text if priority is set
        string displayText = Date.Humanize();
        output.Content.SetContent(displayText);
    }
}
