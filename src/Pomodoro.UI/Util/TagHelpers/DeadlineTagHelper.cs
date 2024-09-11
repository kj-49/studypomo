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

            if (timeToDeadline.TotalDays < 1)
            {
                textColor = "text-danger";
            }
            else
            {
                textColor = "text-muted";
            }
        }



        output.Attributes.SetAttribute("class", textColor);

        // Modify the text if priority is set
        string displayText = Date.Humanize();
        output.Content.SetContent(displayText);
    }
}
