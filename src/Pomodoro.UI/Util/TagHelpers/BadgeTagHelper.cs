using Microsoft.AspNetCore.Razor.TagHelpers;
using Pomodoro.Library.Models.Tables.LabelEntities;

namespace Pomodoro.UI.Util.TagHelpers;

public class BadgeTagHelper : TagHelper
{
    public string Color { get; set; }
    public string Text { get; set; }
    public bool Priority { get; set; } = false; // Default value is false

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "span";

        output.Attributes.SetAttribute("class", "badge me-1");
        output.Attributes.SetAttribute("style", $"background-color: {Color}");

        // Modify the text if priority is set
        string displayText = Priority ? $"<span class=\"fw-normal\">Priority:</span> {Text}" : Text;
        output.Content.SetHtmlContent(displayText);
    }
}