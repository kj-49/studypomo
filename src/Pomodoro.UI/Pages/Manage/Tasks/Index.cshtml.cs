using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pomodoro.Library.Authorization;
using Pomodoro.Library.Models.Tables.StudyTaskEntities;
using Pomodoro.Library.Services.Interfaces;

namespace Pomodoro.UI.Pages.Manage.Tasks;

public class IndexModel : PageModel
{
    private readonly IStudyTaskService _studyTaskService;
    private readonly IAuthorizationService _authorizationService;

    public IndexModel(IStudyTaskService studyTaskService, IAuthorizationService authorizationService)
    {
        _studyTaskService = studyTaskService;
        _authorizationService = authorizationService;
    }

    public StudyTask StudyTask { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        StudyTask studyTask = await _studyTaskService.GetAsync(id);
        var authResult = await _authorizationService.AuthorizeAsync(User, studyTask, Operations.Read);

        if (!authResult.Succeeded)
        {
            if (User.Identity.IsAuthenticated)
            {
                return new ForbidResult();
            }
            else
            {
                return new ChallengeResult();
            }
        }

        StudyTask = studyTask;

        return Page();
    }

    public double GetProgressPercentage(DateTime createdDate, DateTime dueDate)
    {
        var totalDuration = (dueDate - createdDate).TotalDays;
        var elapsedDuration = (DateTime.Now - createdDate).TotalDays;
        return (elapsedDuration / totalDuration) * 100;
    }
}
