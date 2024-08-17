using Microsoft.AspNetCore.Razor.TagHelpers;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
using Pomodoro.Library.Models.Tables.StudyTaskEntities;

namespace Pomodoro.UI.Util.TagHelpers;

public class StudyTaskLinkTagHelper : TagHelper
{
    public StudyTask Task { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "a";

        output.Attributes.SetAttribute("class", "me-1");
        output.Attributes.SetAttribute("href", $"/Manage/Tasks/{Task.Id}");

        // Modify the text if priority is set
        string displayText = Task.Name;
        output.Content.SetContent(displayText);
    }
}
