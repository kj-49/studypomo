using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PomodoroLibrary.Models.Identity;
using PomodoroLibrary.Models.Tables.CourseEntities;
using PomodoroLibrary.Models.Tables.LabelEntities;
using PomodoroLibrary.Models.Tables.StudyTaskEntities;
using PomodoroLibrary.Models.Tables.TaskPriorityEntities;
using PomodoroLibrary.Services.Interfaces;

namespace PomodoroUI.Pages.Manage;

public class CourseModel : PageModel
{
    private readonly ICourseService _courseService;
    private readonly ITaskPriorityService _taskPriorityService;
    private readonly ITaskLabelService _taskLabelService;
    private readonly IUserService _userService;

    public CourseModel(ICourseService courseService, ITaskPriorityService taskPriorityService, ITaskLabelService taskLabelService, IUserService userService)
    {
        _courseService = courseService;
        _taskPriorityService = taskPriorityService;
        _taskLabelService = taskLabelService;
        _userService = userService;
    }

    public Course Course { get; set; }

    [BindProperty]
    public StudyTaskCreate StudyTaskCreate { get; set; }
    public ICollection<TaskPriority> TaskPriorities { get; set; }
    public ICollection<TaskLabel> TaskLabels { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        ApplicationUser? user = await _userService.GetCurrentUserAsync();

        if (user == null) return Challenge();

        Course = await _courseService.GetAsync(id);
        TaskPriorities = await _taskPriorityService.GetAllAsync();
        TaskLabels = await _taskLabelService.GetAllAsync(user.Id);

        return Page();
    }

    public async Task OnPostAsync()
    {

    }
}
