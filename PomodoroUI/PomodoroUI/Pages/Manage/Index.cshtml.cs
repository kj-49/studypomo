using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PomodoroLibrary.Models.Tables.StudyTaskEntities;
using PomodoroLibrary.Services.Interfaces;

namespace PomodoroUI.Pages.Manage;

public class IndexModel : PageModel
{
    private readonly IStudyTaskService _studyTaskService;
    private readonly IUserService _userService;

    public IndexModel(IStudyTaskService studyTaskService, IUserService userService)
    {
        _studyTaskService = studyTaskService;
        _userService = userService;
    }

    public ICollection<StudyTask> StudyTasks { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userService.GetCurrentUserAsync();

        if (user == null) return Challenge();

        StudyTasks = await _studyTaskService.GetAllAsync(user.Id);

        return Page();
    }
}
