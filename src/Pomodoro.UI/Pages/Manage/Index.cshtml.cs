using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic;
using Pomodoro.Library.Models.Tables.CourseEntities;
using Pomodoro.Library.Models.Tables.LabelEntities;
using Pomodoro.Library.Models.Tables.StudyTaskEntities;
using Pomodoro.Library.Models.Tables.TaskPriorityEntities;
using Pomodoro.Library.Services.Interfaces;

namespace Pomodoro.UI.Pages.Manage;

public class IndexModel : PageModel
{
    private readonly IStudyTaskService _studyTaskService;
    private readonly IUserService _userService;
    private readonly ICourseService _courseService;
    private readonly ITaskPriorityService _taskPriorityService;
    private readonly ITaskLabelService _taskLabelService;

    public IndexModel(IStudyTaskService studyTaskService, IUserService userService, ICourseService courseService, ITaskPriorityService taskPriorityService, ITaskLabelService taskLabelService)
    {
        _studyTaskService = studyTaskService;
        _userService = userService;
        _courseService = courseService;
        _taskPriorityService = taskPriorityService;
        _taskLabelService = taskLabelService;
    }

    public ICollection<Course> Courses { get; set; }
    [BindProperty]
    public CourseCreate CourseCreate { get; set; }

    public ICollection<TaskPriority> TaskPriorities { get; set; }
    public ICollection<TaskLabel> TaskLabels { get; set; }

    public ICollection<StudyTask> StudyTasks { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userService.GetCurrentUserAsync();

        if (user == null) return Challenge();

        Courses = await _courseService.GetAllAsync(user.Id);
        StudyTasks = await _studyTaskService.GetAllAsync(user.Id);

        TaskPriorities = await _taskPriorityService.GetAllAsync();
        TaskLabels = await _taskLabelService.GetAllAsync(user.Id);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await _courseService.CreateAsync(CourseCreate);

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostUpdateStudyTaskAsync(StudyTaskUpdate studyTaskUpdate)
    {
        await _studyTaskService.UpdateAsync(studyTaskUpdate);

        return RedirectToPage(new { id = studyTaskUpdate.CourseId });
    }
}
