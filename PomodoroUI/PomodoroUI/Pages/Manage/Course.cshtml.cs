using Htmx;
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
    private readonly IStudyTaskService _studyTaskService;

    public CourseModel(ICourseService courseService, ITaskPriorityService taskPriorityService, ITaskLabelService taskLabelService, IUserService userService, IStudyTaskService studyTaskService)
    {
        _courseService = courseService;
        _taskPriorityService = taskPriorityService;
        _taskLabelService = taskLabelService;
        _userService = userService;
        _studyTaskService = studyTaskService;
    }

    public Course Course { get; set; }

    [BindProperty]
    public StudyTaskCreate StudyTaskCreate { get; set; }
    [BindProperty]
    public StudyTaskUpdate StudyTaskUpdate { get; set; }
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

    public async Task<IActionResult> OnPostCreateStudyTaskAsync()
    {
        await _studyTaskService.CreateAsync(StudyTaskCreate);

        return RedirectToPage(new { id = StudyTaskCreate.CourseId });
    }

    public async Task<IActionResult> OnPostUpdateStudyTaskAsync()
    {
        await _studyTaskService.UpdateAsync(StudyTaskUpdate);

        return RedirectToPage( new { id = StudyTaskUpdate.CourseId });
    }
}
