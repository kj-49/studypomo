using Htmx;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudyPomo.Library.Authorization;
using StudyPomo.Library.Models.Identity;
using StudyPomo.Library.Models.Tables.CourseEntities;
using StudyPomo.Library.Models.Tables.LabelEntities;
using StudyPomo.Library.Models.Tables.StudyTaskEntities;
using StudyPomo.Library.Models.Tables.TaskPriorityEntities;
using StudyPomo.Library.Services.Interfaces;
using StudyPomo.UI.Util.PageModels;

namespace StudyPomo.UI.Pages.Manage.Courses;

public class IndexModel : BaseModel
{
    private readonly ICourseService _courseService;
    private readonly ITaskPriorityService _taskPriorityService;
    private readonly ITaskLabelService _taskLabelService;
    private readonly IUserService _userService;
    private readonly IStudyTaskService _studyTaskService;
    private readonly IAuthorizationService _authorizationService;
    private readonly UserManager<ApplicationUser> _userManager;

    public IndexModel(ICourseService courseService,
        ITaskPriorityService taskPriorityService,
        ITaskLabelService taskLabelService,
        IUserService userService,
        IStudyTaskService studyTaskService,
        IAuthorizationService authorizationService,
        UserManager<ApplicationUser> userManager)
        : base(userService)
    {
        _courseService = courseService;
        _taskPriorityService = taskPriorityService;
        _taskLabelService = taskLabelService;
        _userService = userService;
        _studyTaskService = studyTaskService;
        _authorizationService = authorizationService;
        _userManager = userManager;
    }

    public Course Course { get; set; }

    [BindProperty]
    public StudyTaskCreate StudyTaskCreate { get; set; }
    [BindProperty]
    public CourseUpdate CourseUpdate { get; set; }
    public SelectList TaskPriorities { get; set; }
    public ICollection<TaskLabel> TaskLabels { get; set; }
    public ICollection<Course> Courses { get; set; }

    protected override async Task<TimeZoneInfo> ResolveTimeZone()
    {
        ApplicationUser? user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            throw new Exception("User not found.");
        }

        return TimeZoneInfo.FindSystemTimeZoneById(user.TimeZoneId ?? SD.UTC);
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        ApplicationUser? user = await _userService.GetCurrentUserAsync();

        if (user == null) return Challenge();
        
        Course course = await _courseService.GetAsync(id);

        var authResult = await _authorizationService.AuthorizeAsync(User, course, Operations.Read);

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

        await PopulateFields(userId: user.Id, courseId: id);

        await InitializeTimeZoneAsync();

        return Page();
    }

    public async Task PopulateFields(int userId, int courseId)
    {
        Course = await _courseService.GetAsync(courseId);
        TaskPriorities = new SelectList(await _taskPriorityService.GetAllAsync(), "Id", "Level");
        TaskLabels = await _taskLabelService.GetAllAsync(userId);
        Courses = await _courseService.GetAllAsync(userId);
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

        return RedirectToPage(new { id = StudyTaskCreate.CourseId });
    }

    public async Task<IActionResult> OnPostUpdateStudyTaskAsync(StudyTaskUpdate studyTaskUpdate, int courseId)
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

        // Incase courseId value changes, let's redirect to home page.
        return RedirectToPage("/Manage/Index");
    }

    public async Task<IActionResult> OnPostCompleteTaskAsync(int studyTaskId, int courseId)
    {
        if (!Request.IsHtmx())
        {
            return new EmptyResult();
        }

        StudyTask studyTask = await _studyTaskService.GetAsync(studyTaskId);

        var authResult = await _authorizationService.AuthorizeAsync(User, studyTask, Operations.Update);

        if (!authResult.Succeeded)
        {
            return new EmptyResult();
        }
        
        await _studyTaskService.CompleteAsync(studyTaskId);

        ApplicationUser? user = await _userService.GetCurrentUserAsync();

        if (user == null) return Challenge();

        await PopulateFields(userId: user.Id, courseId: courseId);

        return Partial("Partials/_Dynamic", this);

    }

    public async Task<IActionResult> OnPostUncompleteTaskAsync(int studyTaskId, int courseId)
    {
        if (!Request.IsHtmx())
        {
            return new EmptyResult();
        }

        StudyTask studyTask = await _studyTaskService.GetAsync(studyTaskId);

        var authResult = await _authorizationService.AuthorizeAsync(User, studyTask, Operations.Update);

        if (!authResult.Succeeded)
        {
            return new EmptyResult();
        }

        await _studyTaskService.UncompleteAsync(studyTaskId);

        ApplicationUser? user = await _userService.GetCurrentUserAsync();

        if (user == null) return Challenge();

        await PopulateFields(userId: user.Id, courseId: courseId);

        return Partial("Partials/_Dynamic", this);

    }

    public async Task<IActionResult> OnPostArchiveStudyTaskAsync(int studyTaskId, int courseId)
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

        return RedirectToPage(new { id = courseId });
    }

    public async Task<IActionResult> OnPostUpdateCourseAsync()
    {
        Course course = await _courseService.GetAsync(CourseUpdate.Id);

        var authResult = await _authorizationService.AuthorizeAsync(User, course, Operations.Update);

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

        await _courseService.UpdateAsync(CourseUpdate);

        return RedirectToPage(new { id = CourseUpdate.Id });
    }


    public async Task<IActionResult> OnGetProgressStatsAsync(int courseId)
    {
        ApplicationUser? user = await _userService.GetCurrentUserAsync();

        if (user == null) return Challenge();

        Course course = await _courseService.GetAsync(courseId);

        var authResult = await _authorizationService.AuthorizeAsync(User, course, Operations.Read);

        if (!authResult.Succeeded)
        {
            return new EmptyResult();
        }

        var completedTasks = course.StudyTasks
            .Where(t => t.DateCompleted.HasValue)
            .GroupBy(t => t.DateCompleted!.Value.Date)
            .Select(g => new
            {
                Date = g.Key,
                Count = g.Count()
            })
            .OrderBy(g => g.Date)
            .ToList();

        // Prepare the data for the chart
        var data = completedTasks.Select(ct => new
        {
            Date = ct.Date.ToString("yyyy-MM-dd"),  // Format the date as needed
            Count = ct.Count
        });

        return new JsonResult(data);

    }
}
