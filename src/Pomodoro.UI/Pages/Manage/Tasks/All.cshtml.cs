using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Pomodoro.Library.Authorization;
using Pomodoro.Library.Models.Identity;
using Pomodoro.Library.Models.Tables.CourseEntities;
using Pomodoro.Library.Models.Tables.LabelEntities;
using Pomodoro.Library.Models.Tables.StudyTaskEntities;
using Pomodoro.Library.Models.Tables.TaskPriorityEntities;
using Pomodoro.Library.Services;
using Pomodoro.Library.Services.Interfaces;
using Pomodoro.UI.Util.PageModels;

namespace Pomodoro.UI.Pages.Manage.Tasks;

public class AllModel : BaseModel
{
    private readonly IUserService _userService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IStudyTaskService _studyTaskService;
    private readonly ITaskPriorityService _taskPriorityService;
    private readonly ITaskLabelService _taskLabelService;
    private readonly IAuthorizationService _authorizationService;
    private readonly ICourseService _courseService;

    public AllModel(IUserService userService, UserManager<ApplicationUser> userManager, IStudyTaskService studyTaskService, ITaskPriorityService taskPriorityService, ITaskLabelService taskLabelService, IAuthorizationService authorizationService, ICourseService courseService) : base(userService)
    {
        _userService = userService;
        _userManager = userManager;
        _studyTaskService = studyTaskService;
        _taskPriorityService = taskPriorityService;
        _taskLabelService = taskLabelService;
        _authorizationService = authorizationService;
        _courseService = courseService;
    }

    protected override async Task<TimeZoneInfo> ResolveTimeZone()
    {
        ApplicationUser? user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            throw new Exception("User not found.");
        }

        return TimeZoneInfo.FindSystemTimeZoneById(user.TimeZoneId ?? SD.UTC);
    }

    [BindProperty]
    public StudyTaskCreate StudyTaskCreate { get; set; }

    public ICollection<StudyTask> StudyTasks { get; set; }
    public SelectList TaskPriorities { get; set; }
    public ICollection<TaskLabel> TaskLabels { get; set; }
    public ICollection<Course> Courses { get; set; }

    public FilterOptions Filter { get; set; }
    public bool FilterActive { get; set; }

    public async Task<IActionResult> OnGetAsync(FilterOptions filter)
    {

        Filter = filter;

        var user = await _userService.GetCurrentUserAsync();

        if (user == null) return Challenge();
        StudyTasks = await _studyTaskService.GetAllAsync(user.Id);

        TaskPriorities = new SelectList(await _taskPriorityService.GetAllAsync(), "Id", "Level");
        TaskLabels = await _taskLabelService.GetAllAsync(user.Id);
        Courses = await _courseService.GetAllAsync(user.Id);

        await InitializeTimeZoneAsync();

        ApplyFilter();

        return Page();
    }

    public async Task<IActionResult> OnPostArchiveStudyTaskAsync(int studyTaskId)
    {
        StudyTask studyTask = await _studyTaskService.GetAsync(studyTaskId);

        // Authorize
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

    public async Task<IActionResult> OnPostUpdateStudyTaskAsync(StudyTaskUpdate studyTaskUpdate)
    {
        // Authorize
        StudyTask studyTask = await _studyTaskService.GetAsync(studyTaskUpdate.Id);

        // TODO: Authorize
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

    public class FilterOptions
    {
        public int? TaskPriorityId { get; set; }
        public List<int> CourseIds { get; set; } = [];
        public List<int> TaskLabelIds { get; set; } = [];
        public string? SearchQuery { get; set; }
        public bool DueDateDescending { get; set; }
    }

    private void ApplyFilter()
    {
        if (Filter.TaskPriorityId.HasValue)
        {
            FilterActive = true;
            StudyTasks = StudyTasks.Where(u => u.TaskPriorityId == Filter.TaskPriorityId).ToList();
        }
        if (Filter.CourseIds.Any())
        {
            FilterActive = true;
            StudyTasks = StudyTasks.Where(u => Filter.CourseIds.Any(c => c == u.CourseId)).ToList();
        }
        if (Filter.TaskLabelIds.Any())
        {
            FilterActive = true;
            StudyTasks = StudyTasks
                .Where(u => u.TaskLabels.Select(t => t.Id)
                .Intersect(Filter.TaskLabelIds)
                .Any())
                .ToList();
        }
        if (!string.IsNullOrWhiteSpace(Filter.SearchQuery))
        {
            FilterActive = true;
            StudyTasks = StudyTasks.Where(u => u.Name.Contains(Filter.SearchQuery)).ToList();
        }
        if (Filter.DueDateDescending)
        {
            FilterActive = true;
            StudyTasks = StudyTasks
                .OrderByDescending(u => u.Deadline.HasValue) // Sort nulls last
                .ThenByDescending(u => u.Deadline)           // Then sort by deadline descending
                .ToList();
        }
        else
        {
            StudyTasks = StudyTasks
                .OrderByDescending(u => u.Deadline.HasValue) // Sort nulls last
                .ThenBy(u => u.Deadline)           // Then sort by deadline ascending
                .ToList();
        }
    }

}
