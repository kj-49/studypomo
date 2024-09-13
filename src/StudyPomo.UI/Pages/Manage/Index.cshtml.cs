using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualBasic;
using StudyPomo.Library.Authorization;
using StudyPomo.Library.Models.Identity;
using StudyPomo.Library.Models.Tables.CourseEntities;
using StudyPomo.Library.Models.Tables.LabelEntities;
using StudyPomo.Library.Models.Tables.StudyTaskEntities;
using StudyPomo.Library.Models.Tables.TaskPriorityEntities;
using StudyPomo.Library.Services.Interfaces;
using StudyPomo.UI.Util.PageModels;
using System.Runtime.CompilerServices;

namespace StudyPomo.UI.Pages.Manage;

public class IndexModel : BaseModel
{
    private readonly IStudyTaskService _studyTaskService;
    private readonly IUserService _userService;
    private readonly ICourseService _courseService;
    private readonly ITaskPriorityService _taskPriorityService;
    private readonly ITaskLabelService _taskLabelService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IAuthorizationService _authorizationService;

    private readonly ILogger<IndexModel> _logger;

    public IndexModel(IStudyTaskService studyTaskService,
        IUserService userService,
        ICourseService courseService,
        ITaskPriorityService taskPriorityService,
        ITaskLabelService taskLabelService,
        UserManager<ApplicationUser> userManager,
        IAuthorizationService authorizationService,
        ILogger<IndexModel> logger)
        : base(userService)
    {
        _studyTaskService = studyTaskService;
        _userService = userService;
        _courseService = courseService;
        _taskPriorityService = taskPriorityService;
        _taskLabelService = taskLabelService;
        _userManager = userManager;
        _authorizationService = authorizationService;
        _logger = logger;
    }

    public ICollection<Course> Courses { get; set; }

    
    public CourseCreate CourseCreate { get; set; }

    [BindProperty]
    public StudyTaskCreate StudyTaskCreate { get; set; }

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
        _logger.LogInformation("Manage page accessed.");

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
        ApplicationUser? user = await _userService.GetCurrentUserAsync();
        if (user == null) return Challenge();

        // Need to ensure the created task is for a CourseId that the current user owns.
        StudyTask studyTask = StudyTaskCreate.ToEntity(user.Id, TimeZoneInfo.FindSystemTimeZoneById(user.TimeZoneId ?? SD.UTC));
        var authResult = await _authorizationService.AuthorizeAsync(User, studyTask, Operations.Create);

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

        await _studyTaskService.CreateAsync(StudyTaskCreate);

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostArchiveStudyTaskAsync(int studyTaskId)
    {
        StudyTask studyTask = await _studyTaskService.GetAsync(studyTaskId);

        var authResult = await _authorizationService.AuthorizeAsync(User, studyTask, Operations.Update);

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
        StudyTask studyTask = await _studyTaskService.GetAsync(studyTaskUpdate.Id);

        var authResult = await _authorizationService.AuthorizeAsync(User, studyTask, Operations.Update);

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

        await _studyTaskService.UpdateAsync(studyTaskUpdate);

        return RedirectToPage(new { id = studyTaskUpdate.CourseId });
    }

    public async Task<IActionResult> OnPostCreateCourseAsync(CourseCreate courseCreate)
    {
        ApplicationUser? user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge();

        Course course = courseCreate.ToEntity(user.Id);

        var authResult = await _authorizationService.AuthorizeAsync(User, course, Operations.Create);

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

        await _courseService.CreateAsync(courseCreate);

        return RedirectToPage();
    }
}
