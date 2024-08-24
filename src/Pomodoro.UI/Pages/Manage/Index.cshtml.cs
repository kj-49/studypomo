using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic;
using Pomodoro.Library.Models.Identity;
using Pomodoro.Library.Models.Tables.CourseEntities;
using Pomodoro.Library.Models.Tables.LabelEntities;
using Pomodoro.Library.Models.Tables.StudyTaskEntities;
using Pomodoro.Library.Models.Tables.TaskPriorityEntities;
using Pomodoro.Library.Services.Interfaces;
using Pomodoro.UI.Util.PageModels;

namespace Pomodoro.UI.Pages.Manage;

public class IndexModel : BaseModel
{
    private readonly IStudyTaskService _studyTaskService;
    private readonly IUserService _userService;
    private readonly ICourseService _courseService;
    private readonly ITaskPriorityService _taskPriorityService;
    private readonly ITaskLabelService _taskLabelService;
    private readonly UserManager<ApplicationUser> _userManager;

    public IndexModel(IStudyTaskService studyTaskService,
        IUserService userService,
        ICourseService courseService,
        ITaskPriorityService taskPriorityService,
        ITaskLabelService taskLabelService,
        UserManager<ApplicationUser> userManager)
        : base (userService)
    {
        _studyTaskService = studyTaskService;
        _userService = userService;
        _courseService = courseService;
        _taskPriorityService = taskPriorityService;
        _taskLabelService = taskLabelService;
        _userManager = userManager;
    }

    public ICollection<Course> Courses { get; set; }
    [BindProperty]
    public CourseCreate CourseCreate { get; set; }

    public ICollection<TaskPriority> TaskPriorities { get; set; }
    public ICollection<TaskLabel> TaskLabels { get; set; }

    public ICollection<StudyTask> StudyTasks { get; set; }

    protected override async Task<TimeZoneInfo> ResolveTimeZone()
    {
        ApplicationUser? user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            throw new Exception("User not found.");
        }

        return TimeZoneInfo.FindSystemTimeZoneById(user.TimeZoneId ?? SD.UTC);
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userService.GetCurrentUserAsync();

        if (user == null) return Challenge();

        Courses = await _courseService.GetAllAsync(user.Id);
        StudyTasks = await _studyTaskService.GetAllAsync(user.Id);

        TaskPriorities = await _taskPriorityService.GetAllAsync();
        TaskLabels = await _taskLabelService.GetAllAsync(user.Id);

        await InitializeTimeZoneAsync();

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
