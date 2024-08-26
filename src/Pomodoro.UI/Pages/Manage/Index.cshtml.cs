using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualBasic;
using Pomodoro.Library.Authorization;
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
    private readonly IAuthorizationService _authorizationService;

    public IndexModel(IStudyTaskService studyTaskService,
        IUserService userService,
        ICourseService courseService,
        ITaskPriorityService taskPriorityService,
        ITaskLabelService taskLabelService,
        UserManager<ApplicationUser> userManager,
        IAuthorizationService authorizationService)
        : base(userService)
    {
        _studyTaskService = studyTaskService;
        _userService = userService;
        _courseService = courseService;
        _taskPriorityService = taskPriorityService;
        _taskLabelService = taskLabelService;
        _userManager = userManager;
        _authorizationService = authorizationService;
    }

    public ICollection<Course> Courses { get; set; }
    [BindProperty]
    public CourseCreate CourseCreate { get; set; }

    public SelectList TaskPriorities { get; set; }
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

        TaskPriorities = new SelectList(await _taskPriorityService.GetAllAsync(), "Id", "Level");
        TaskLabels = await _taskLabelService.GetAllAsync(user.Id);

        await InitializeTimeZoneAsync();

        return Page();
    }

    public async Task<IActionResult> OnPostCreateStudyTaskAsync()
    {
        await _courseService.CreateAsync(CourseCreate);

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostArchiveStudyTaskAsync(int studyTaskId)
    {
        StudyTask studyTask = await _studyTaskService.GetAsync(studyTaskId);

        // Authorize
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

        await _studyTaskService.ArchiveAsync(studyTaskId);

        return RedirectToPage();
    }


    public async Task<IActionResult> OnPostUpdateStudyTaskAsync(StudyTaskUpdate studyTaskUpdate)
    {
        await _studyTaskService.UpdateAsync(studyTaskUpdate);

        return RedirectToPage(new { id = studyTaskUpdate.CourseId });
    }
}
