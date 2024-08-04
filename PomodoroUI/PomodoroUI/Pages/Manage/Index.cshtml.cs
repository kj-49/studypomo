using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PomodoroLibrary.Models.Tables.CourseEntities;
using PomodoroLibrary.Models.Tables.StudyTaskEntities;
using PomodoroLibrary.Services.Interfaces;

namespace PomodoroUI.Pages.Manage;

public class IndexModel : PageModel
{
    private readonly IStudyTaskService _studyTaskService;
    private readonly IUserService _userService;
    private readonly ICourseService _courseService;

    public IndexModel(IStudyTaskService studyTaskService, IUserService userService, ICourseService courseService)
    {
        _studyTaskService = studyTaskService;
        _userService = userService;
        _courseService = courseService;
    }

    public ICollection<StudyTask> StudyTasks { get; set; }
    public ICollection<Course> Courses { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userService.GetCurrentUserAsync();

        if (user == null) return Challenge();

        StudyTasks = await _studyTaskService.GetAllAsync(user.Id);
        Courses = await _courseService.GetAllAsync(user.Id);

        return Page();
    }
}
