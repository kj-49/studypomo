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
using StudyPomo.Library.Services;
using StudyPomo.Library.Services.Interfaces;
using StudyPomo.UI.Util.PageModels;

namespace StudyPomo.UI.Pages.Manage.Tasks;

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
        Filter = filter ?? new FilterOptions();

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

        return RedirectToPage();
    }


    public async Task<IActionResult> OnPostCompleteStudyTaskAsync(int id)
    {

        StudyTask studyTask = await _studyTaskService.GetAsync(id);

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

        await _studyTaskService.CompleteAsync(id);

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostUncompleteStudyTaskAsync(int id)
    {
        StudyTask studyTask = await _studyTaskService.GetAsync(id);

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

        await _studyTaskService.UncompleteAsync(id);

        return RedirectToPage();
    }


    public class FilterOptions
    {
        public int? TaskPriorityId { get; set; }
        public List<int> CourseIds { get; set; } = [];
        public List<int> TaskLabelIds { get; set; } = [];
        public string? SearchQuery { get; set; }
        public bool DueDateDescending { get; set; } = false;
        /// <summary>
        /// If null, no ordering is applied.
        /// </summary>
        public bool? OrderByCompleted { get; set; } = false;

        public bool MatchesDefault()
        {
            var defaultOptions = new FilterOptions();

            return TaskPriorityId == defaultOptions.TaskPriorityId
                && CourseIds.SequenceEqual(defaultOptions.CourseIds)
                && TaskLabelIds.SequenceEqual(defaultOptions.TaskLabelIds)
                && SearchQuery == defaultOptions.SearchQuery
                && DueDateDescending == defaultOptions.DueDateDescending
                && OrderByCompleted == defaultOptions.OrderByCompleted;
        }
    }
    private void ApplyFilter()
    {
        if (Filter == null) return;

        FilterActive = !Filter.MatchesDefault();

        if (Filter.TaskPriorityId.HasValue)
        {
            StudyTasks = StudyTasks.Where(u => u.TaskPriorityId == Filter.TaskPriorityId).ToList();
        }
        if (Filter.CourseIds.Any())
        {
            StudyTasks = StudyTasks.Where(u => Filter.CourseIds.Any(c => c == u.CourseId)).ToList();
        }
        if (Filter.TaskLabelIds.Any())
        {
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
        if (Filter.OrderByCompleted.HasValue)
        {
            if (Filter.OrderByCompleted.Value)
            {
                StudyTasks = StudyTasks
                    .OrderByDescending(u => u.Completed)
                    .ToList();
            } else
            {
                StudyTasks = StudyTasks
                    .OrderByDescending(u => !u.Completed)
                    .ToList();
            }
        }
    }

}
