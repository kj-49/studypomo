using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pomodoro.Library.Models.Tables.StudyTaskEntities;
using Pomodoro.Library.Services.Interfaces;

namespace Pomodoro.UI.Pages.Manage.Tasks;

public class IndexModel : PageModel
{
    private readonly IStudyTaskService _studyTaskService;

    public IndexModel(IStudyTaskService studyTaskService)
    {
        _studyTaskService = studyTaskService;
    }

    public StudyTask StudyTask { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        StudyTask = await _studyTaskService.GetAsync(id);

        return Page();
    }

    public double GetProgressPercentage(DateTime createdDate, DateTime dueDate)
    {
        var totalDuration = (dueDate - createdDate).TotalDays;
        var elapsedDuration = (DateTime.Now - createdDate).TotalDays;
        return (elapsedDuration / totalDuration) * 100;
    }
}
