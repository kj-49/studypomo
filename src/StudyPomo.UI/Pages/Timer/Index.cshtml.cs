
using AutoMapper;
using Htmx;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Build.Execution;
using Microsoft.Build.Framework;
using StudyPomo.Library.Authorization;
using StudyPomo.Library.Data;
using StudyPomo.Library.Data.Interfaces;
using StudyPomo.Library.Models.Identity;
using StudyPomo.Library.Models.Tables.CourseEntities;
using StudyPomo.Library.Models.Tables.LabelEntities;
using StudyPomo.Library.Models.Tables.StudyTaskEntities;
using StudyPomo.Library.Models.Tables.TaskPriorityEntities;
using StudyPomo.Library.Services;
using StudyPomo.Library.Services.Interfaces;
using StudyPomo.UI.Util.PageModels;
using System.Runtime.CompilerServices;

namespace StudyPomo.UI.Pages.Timer;

public class IndexModel : BaseModel
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStudyTaskService _studyTaskService;
    private readonly ITaskPriorityService _taskPriorityService;
    private readonly ITaskLabelService _taskLabelService;
    private readonly IMapper _mapper;
    private readonly IAuthorizationService _authorizationService;
    private readonly IUserService _userService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ICourseService _courseService;

    private static string _workingTaskIdKey = "WORKING_TASK_ID";

    public IndexModel(
        IUserService userService,
        IUnitOfWork unitOfWork,
        IStudyTaskService studyTaskService,
        IMapper mapper,
        ITaskPriorityService taskPriorityService,
        ITaskLabelService taskLabelService,
        IAuthorizationService authorizationService,
        UserManager<ApplicationUser> userManager,
        ICourseService courseService)
        : base(userService)
    {
        _userService = userService;
        _unitOfWork = unitOfWork;
        _studyTaskService = studyTaskService;
        _mapper = mapper;
        _taskPriorityService = taskPriorityService;
        _taskLabelService = taskLabelService;
        _authorizationService = authorizationService;
        _userManager = userManager;
        _courseService = courseService;
    }

    public SelectList TaskPriorities { get; set; }

    public int? WorkingStudyTaskId { get; set; }

    public ICollection<StudyTask> UncompletedStudyTasks { get; set; }

    [BindProperty]
    public StudyTaskCreate StudyTaskCreate { get; set; }
    [BindProperty]
    public StudyTaskUpdate StudyTaskUpdate { get; set; }

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


    public async Task<IActionResult> OnGetAsync()
    {
        ApplicationUser? user = await _userService.GetCurrentUserAsync();

        if (user == null) return Challenge();

        await PopulateFields(user.Id);

        StudyTaskCreate = new StudyTaskCreate();
        StudyTaskUpdate = new StudyTaskUpdate();

        return Page();
    }

    public async Task PopulateFields(int userId)
    {
        await InitializeTimeZoneAsync();

        WorkingStudyTaskId = HttpContext.Session.GetInt32(_workingTaskIdKey);

        IEnumerable<StudyTask> studyTasks = await _studyTaskService.GetAllAsync(userId);
        UncompletedStudyTasks = studyTasks.Where(u => !u.Completed).ToList();

        if (HttpContext.Session.GetInt32(_workingTaskIdKey) != null)
        {
            WorkingStudyTaskId = HttpContext.Session.GetInt32(_workingTaskIdKey);
        }
        else
        {
            WorkingStudyTaskId = UncompletedStudyTasks.Prioritize().FirstOrDefault()?.Id;
        }

        TaskPriorities = new SelectList(await _taskPriorityService.GetAllAsync(), "Id", "Level");
        TaskLabels = await _taskLabelService.GetAllAsync(userId);
        Courses = await _courseService.GetAllAsync(userId);
    }

    public async Task<IActionResult> OnPostCreateStudyTaskAsync()
    {
        if (!Request.IsHtmx())
        {
            return new EmptyResult();
        }

        ApplicationUser? user = await _userService.GetCurrentUserAsync();
        if (user == null) return Challenge();

        StudyTask studyTask = StudyTaskCreate.ToEntity(user.Id, TimeZoneInfo.FindSystemTimeZoneById(user.TimeZoneId ?? SD.UTC));

        var authResult = await _authorizationService.AuthorizeAsync(User, studyTask, Operations.Create);

        if (!authResult.Succeeded)
        {
            return new EmptyResult();
        }

        await _studyTaskService.CreateAsync(StudyTaskCreate);

        await PopulateFields(user.Id);

        return Partial("Partials/_Dynamic", this);
    }

    public async Task<IActionResult> OnPostRemoveStudyTaskAsync(int id)
    {
        if (!Request.IsHtmx())
        {
            return new EmptyResult();
        }

        ApplicationUser? user = await _userService.GetCurrentUserAsync();
        if (user == null) return Challenge();

        StudyTask studyTask = await _studyTaskService.GetAsync(id);

        var authResult = await _authorizationService.AuthorizeAsync(User, studyTask, Operations.Delete);

        if (!authResult.Succeeded)
        {
            return new EmptyResult();
        }

        await _studyTaskService.RemoveAsync(id);

        if (HttpContext.Session.GetInt32(_workingTaskIdKey) == WorkingStudyTaskId)
        {
            HttpContext.Session.Remove(_workingTaskIdKey);
        }

        await PopulateFields(user.Id);

        return Partial("Partials/_Dynamic", this);
    }

    public async Task<IActionResult> OnPostArchiveStudyTaskAsync(int id)
    {
        if (!Request.IsHtmx())
        {
            return new EmptyResult();
        }

        ApplicationUser? user = await _userService.GetCurrentUserAsync();
        if (user == null) return Challenge();

        StudyTask studyTask = await _studyTaskService.GetAsync(id);

        var authResult = await _authorizationService.AuthorizeAsync(User, studyTask, Operations.Update);

        if (!authResult.Succeeded)
        {
            return new EmptyResult();
        }

        await _studyTaskService.ArchiveAsync(id);

        await PopulateFields(user.Id);

        return Partial("Partials/_Dynamic", this);
    }

    public async Task<IActionResult> OnPostUpdateStudyTaskAsync()
    {
        if (!Request.IsHtmx())
        {
            return new EmptyResult();
        }

        StudyTask studyTask = await _studyTaskService.GetAsync(StudyTaskUpdate.Id);

        var authResult = await _authorizationService.AuthorizeAsync(User, studyTask, Operations.Update);

        if (!authResult.Succeeded)
        {
            return new EmptyResult();
        }

        await _studyTaskService.UpdateAsync(StudyTaskUpdate);

        ApplicationUser? user = await _userService.GetCurrentUserAsync();

        if (user == null) return Challenge();

        await PopulateFields(user.Id);

        return Partial("Partials/_Dynamic", this);
    }

    public async Task<IActionResult> OnGetStudyTaskUpdateAsync(int id)
    {
        if (!Request.IsHtmx())
        {
            return new EmptyResult();
        }

        ApplicationUser? user = await _userService.GetCurrentUserAsync();
        if (user == null) return Challenge();

        StudyTask studyTask = await _studyTaskService.GetAsync(id);

        var authResult = await _authorizationService.AuthorizeAsync(User, studyTask, Operations.Update);

        if (!authResult.Succeeded)
        {
            return new EmptyResult();
        }

        StudyTaskUpdate = _mapper.Map<StudyTaskUpdate>(studyTask);

        await PopulateFields(user.Id);

        return Partial("Partials/_StudyTaskUpdate", this);

    }

    public async Task<IActionResult> OnPostCompleteTaskAsync(int id)
    {
        if (!Request.IsHtmx())
        {
            return new EmptyResult();
        }

        ApplicationUser? user = await _userService.GetCurrentUserAsync();
        if (user == null) return Challenge();

        StudyTask studyTask = await _studyTaskService.GetAsync(id);

        var authResult = await _authorizationService.AuthorizeAsync(User, studyTask, Operations.Update);

        if (!authResult.Succeeded)
        {
            return new EmptyResult();
        }

        await _studyTaskService.CompleteAsync(id);

        if (HttpContext.Session.GetInt32(_workingTaskIdKey) == id)
        {
            HttpContext.Session.Remove(_workingTaskIdKey);
        }

        await PopulateFields(user.Id);

        return Partial("Partials/_Dynamic", this);
    }

    public async Task<IActionResult> OnPostUncompleteTaskAsync(int id)
    {
        ApplicationUser? user = await _userService.GetCurrentUserAsync();
        if (user == null) return Challenge();

        StudyTask studyTask = await _studyTaskService.GetAsync(id);

        var authResult = await _authorizationService.AuthorizeAsync(User, studyTask, Operations.Update);

        if (!authResult.Succeeded)
        {
            return new EmptyResult();
        }

        if (Request.IsHtmx())
        {
            await _studyTaskService.UncompleteAsync(id);

            await PopulateFields(user.Id);

            return Partial("Partials/_Dynamic", this);
        }

        return new EmptyResult();

    }

    public async Task<IActionResult> OnPostSetPreferredThemeAsync(string theme)
    {
        ApplicationUser? user = await _userService.GetCurrentUserAsync();

        if (user == null) return NotFound();

        user.PreferredTheme = theme;

        _unitOfWork.Complete();

        return new OkResult();
    }

    public async Task<IActionResult> OnPostChooseTask(int id)
    {
        if (!Request.IsHtmx())
        {
            return new EmptyResult();
        }

        ApplicationUser? user = await _userService.GetCurrentUserAsync();

        // Dont have to authorize, as linq will look for the Id, it will not be fetched from db.
        HttpContext.Session.SetInt32(_workingTaskIdKey, id);

        await _studyTaskService.UncompleteAsync(id);

        await PopulateFields(user.Id);

        return Partial("Partials/_Dynamic", this);
    }

}
