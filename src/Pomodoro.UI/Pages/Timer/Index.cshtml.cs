
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
using Pomodoro.Library.Authorization;
using Pomodoro.Library.Data;
using Pomodoro.Library.Data.Interfaces;
using Pomodoro.Library.Models.Identity;
using Pomodoro.Library.Models.Tables.LabelEntities;
using Pomodoro.Library.Models.Tables.StudyTaskEntities;
using Pomodoro.Library.Models.Tables.TaskPriorityEntities;
using Pomodoro.Library.Services;
using Pomodoro.Library.Services.Interfaces;
using Pomodoro.UI.Util.PageModels;
using System.Runtime.CompilerServices;

namespace Pomodoro.UI.Pages.Timer;

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

    public IndexModel(
        IUserService userService,
        IUnitOfWork unitOfWork,
        IStudyTaskService studyTaskService,
        IMapper mapper,
        ITaskPriorityService taskPriorityService,
        ITaskLabelService taskLabelService,
        IAuthorizationService authorizationService,
        UserManager<ApplicationUser> userManager)
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
    }

    public ICollection<TaskPriority> TaskPriorities { get; set; }

    public ICollection<StudyTask> StudyTasks { get; set; }
    [BindProperty]
    public StudyTaskCreate StudyTaskCreate { get; set; }
    [BindProperty]
    public StudyTaskUpdate StudyTaskUpdate { get; set; }

    public ICollection<TaskLabel> TaskLabels { get; set; }

    public bool RenderTasksOutOfBand { get; set; }


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
        StudyTasks = (await _studyTaskService.GetAllAsync(userId)).Next(5).ToList();
        TaskPriorities = await _taskPriorityService.GetAllAsync();
        TaskLabels = await _taskLabelService.GetAllAsync(userId);
    }

    public async Task<IActionResult> OnPostCreateStudyTaskAsync()
    {
        ApplicationUser? user = await _userService.GetCurrentUserAsync();
        if (user == null) return Challenge();

        StudyTask studyTask = StudyTaskCreate.ToEntity(user.Id, TimeZoneInfo.FindSystemTimeZoneById(user.TimeZoneId ?? SD.UTC));

        var authResult = await _authorizationService.AuthorizeAsync(User, studyTask, Operations.Create);

        if (!authResult.Succeeded)
        {
            if (User.Identity.IsAuthenticated)
            {
                // TODO: Show status message.
                return Content("");
            }
            else
            {
                // TODO: Show status message.
                return Content("");
            }
        }

        if (Request.IsHtmx()) {

            await _studyTaskService.CreateAsync(StudyTaskCreate);

            await PopulateFields(user.Id);

            return Partial("Partials/_UncompletedStudyTasks", this);
        }

        return Page();
    }

    public async Task<IActionResult> OnPostRemoveStudyTaskAsync(int id)
    {
        ApplicationUser? user = await _userService.GetCurrentUserAsync();
        if (user == null) return Challenge();

        StudyTask studyTask = await _studyTaskService.GetAsync(id);

        var authResult = await _authorizationService.AuthorizeAsync(User, studyTask, Operations.Delete);

        if (!authResult.Succeeded)
        {
            if (User.Identity.IsAuthenticated)
            {
                // TODO: Show status message.
                return Content("");
            }
            else
            {
                // TODO: Show status message.
                return Content("");
            }
        }

        if (Request.IsHtmx())
        {
            await _studyTaskService.RemoveAsync(id);

            await PopulateFields(user.Id);

            return Partial("Partials/_AllStudyTasks", this);
        }

        return Page();
    }

    public async Task<IActionResult> OnPostArchiveStudyTaskAsync(int id)
    {
        ApplicationUser? user = await _userService.GetCurrentUserAsync();
        if (user == null) return Challenge();

        StudyTask studyTask = await _studyTaskService.GetAsync(id);

        var authResult = await _authorizationService.AuthorizeAsync(User, studyTask, Operations.Update);

        if (!authResult.Succeeded)
        {
            if (User.Identity.IsAuthenticated)
            {
                // TODO: Show status message.
                return Content("");
            }
            else
            {
                // TODO: Show status message.
                return Content("");
            }
        }

        if (Request.IsHtmx())
        {
            RenderTasksOutOfBand = true;

            await _studyTaskService.ArchiveAsync(id);

            await PopulateFields(user.Id);

            return Partial("Partials/_AllStudyTasks", this);
        }

        return Page();
    }

    public async Task<IActionResult> OnPostUpdateStudyTaskAsync()
    {
        if (Request.IsHtmx())
        {
            await _studyTaskService.UpdateAsync(StudyTaskUpdate);

            ApplicationUser? user = await _userService.GetCurrentUserAsync();

            if (user == null) return Challenge();

            await PopulateFields(user.Id);

            return Partial("Partials/_AllStudyTasks", this);
        }

        return Page();
    }

    public async Task<IActionResult> OnGetStudyTaskUpdateAsync(int id)
    {
        ApplicationUser? user = await _userService.GetCurrentUserAsync();
        if (user == null) return Challenge();

        StudyTask studyTask = await _studyTaskService.GetAsync(id);

        var authResult = await _authorizationService.AuthorizeAsync(User, studyTask, Operations.Update);

        if (!authResult.Succeeded)
        {
            if (User.Identity.IsAuthenticated)
            {
                // TODO: Show status message.
                return Content("");
            }
            else
            {
                // TODO: Show status message.
                return Content("");
            }
        }

        if (Request.IsHtmx())
        {
            StudyTaskUpdate = _mapper.Map<StudyTaskUpdate>(studyTask);

            await PopulateFields(user.Id);

            return Partial("Partials/_StudyTaskUpdate", this);
        }

        return Page();
    }

    public async Task<IActionResult> OnPostCompleteTaskAsync(int id)
    {
        ApplicationUser? user = await _userService.GetCurrentUserAsync();
        if (user == null) return Challenge();

        StudyTask studyTask = await _studyTaskService.GetAsync(id);

        var authResult = await _authorizationService.AuthorizeAsync(User, studyTask, Operations.Update);

        if (!authResult.Succeeded)
        {
            if (User.Identity.IsAuthenticated)
            {
                // TODO: Show status message.
                return Content("");
            }
            else
            {
                // TODO: Show status message.
                return Content("");
            }
        }

        if (Request.IsHtmx())
        {
            await _studyTaskService.CompleteAsync(id);

            await PopulateFields(user.Id);

            return Partial("Partials/_AllStudyTasks", this);
        }

        return Page();

    }

    public async Task<IActionResult> OnPostUncompleteTaskAsync(int id)
    {
        ApplicationUser? user = await _userService.GetCurrentUserAsync();
        if (user == null) return Challenge();

        StudyTask studyTask = await _studyTaskService.GetAsync(id);

        var authResult = await _authorizationService.AuthorizeAsync(User, studyTask, Operations.Update);

        if (!authResult.Succeeded)
        {
            if (User.Identity.IsAuthenticated)
            {
                // TODO: Show status message.
                return Content("");
            }
            else
            {
                // TODO: Show status message.
                return Content("");
            }
        }

        if (Request.IsHtmx())
        {
            await _studyTaskService.UncompleteAsync(id);

            await PopulateFields(user.Id);

            return Partial("Partials/_AllStudyTasks", this);
        }

        return Page();

    }

    public async Task<IActionResult> OnPostSetPreferredThemeAsync(string theme)
    {
        ApplicationUser? user = await _userService.GetCurrentUserAsync();

        if (user == null) return NotFound();

        user.PreferredTheme = theme;

        _unitOfWork.Complete();

        return new OkResult();
    }
}
