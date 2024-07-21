
using AutoMapper;
using Htmx;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Build.Framework;
using PomodoroLibrary.Data;
using PomodoroLibrary.Data.Interfaces;
using PomodoroLibrary.Models.Identity;
using PomodoroLibrary.Models.Tables.LabelEntities;
using PomodoroLibrary.Models.Tables.StudyTaskEntities;
using PomodoroLibrary.Models.Tables.TaskPriorityEntities;
using PomodoroLibrary.Services.Interfaces;
using System.Runtime.CompilerServices;

namespace PomodoroUI.Pages.Timer;

public class IndexModel : PageModel
{
    private readonly IUserService _userService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStudyTaskService _studyTaskService;
    private readonly ITaskPriorityService _taskPriorityService;
    private readonly ITaskLabelService _taskLabelService;
    private readonly IMapper _mapper;

    public IndexModel(IUserService userService, IUnitOfWork unitOfWork, IStudyTaskService studyTaskService, IMapper mapper, ITaskPriorityService taskPriorityService, ITaskLabelService taskLabelService)
    {
        _userService = userService;
        _unitOfWork = unitOfWork;
        _studyTaskService = studyTaskService;
        _mapper = mapper;
        _taskPriorityService = taskPriorityService;
        _taskLabelService = taskLabelService;
    }

    public ICollection<TaskPriority> TaskPriorities { get; set; }

    public ICollection<StudyTask> StudyTasks { get; set; }
    [BindProperty]
    public StudyTaskCreate StudyTaskCreate { get; set; }
    [BindProperty]
    public StudyTaskUpdate StudyTaskUpdate { get; set; }

    public ICollection<TaskLabel> TaskLabels { get; set; }

    public bool RenderTasksOutOfBand { get; set; }

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
        StudyTasks = await _studyTaskService.GetAllAsync(userId);
        TaskPriorities = await _taskPriorityService.GetAllAsync();
        TaskLabels = await _taskLabelService.GetAllAsync(userId);
    }

    public async Task<IActionResult> OnPostCreateStudyTaskAsync()
    {
        if (Request.IsHtmx()) {

            await _studyTaskService.CreateAsync(StudyTaskCreate);

            ApplicationUser? user = await _userService.GetCurrentUserAsync();

            if (user == null) return Challenge();

            await PopulateFields(user.Id);

            return Partial("Partials/_UncompletedStudyTasks", this);
        }

        return Page();
    }

    public async Task<IActionResult> OnPostRemoveStudyTaskAsync(int id)
    {
        if (Request.IsHtmx())
        {
            await _studyTaskService.RemoveAsync(id);

            ApplicationUser? user = await _userService.GetCurrentUserAsync();

            if (user == null) return Challenge();

            await PopulateFields(user.Id);

            return Partial("Partials/_AllStudyTasks", this);
        }

        return Page();
    }

    public async Task<IActionResult> OnPostArchiveStudyTaskAsync(int id)
    {
        if (Request.IsHtmx())
        {
            RenderTasksOutOfBand = true;

            await _studyTaskService.ArchiveAsync(id);

            ApplicationUser? user = await _userService.GetCurrentUserAsync();

            if (user == null) return Challenge();

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
        
        if (Request.IsHtmx())
        {
            StudyTask? studyTask = await _unitOfWork.StudyTask.GetAsync(u => u.Id == id);

            if (studyTask == null) return NotFound();

            StudyTaskUpdate = _mapper.Map<StudyTaskUpdate>(studyTask);

            ApplicationUser? user = await _userService.GetCurrentUserAsync();

            if (user == null) return Challenge();

            await PopulateFields(user.Id);

            return Partial("Partials/_StudyTaskUpdate", this);
        }

        return Page();
    }

    public async Task<IActionResult> OnPostCompleteTaskAsync(int id)
    {
        if (Request.IsHtmx())
        {
            await _studyTaskService.CompleteAsync(id);

            ApplicationUser? user = await _userService.GetCurrentUserAsync();

            if (user == null) return Challenge();

            await PopulateFields(user.Id);

            return Partial("Partials/_AllStudyTasks", this);
        }

        return Page();

    }

    public async Task<IActionResult> OnPostUncompleteTaskAsync(int id)
    {
        if (Request.IsHtmx())
        {
            await _studyTaskService.UncompleteAsync(id);

            ApplicationUser? user = await _userService.GetCurrentUserAsync();

            if (user == null) return Challenge();

            await PopulateFields(user.Id);

            return Partial("Partials/_AllStudyTasks", this);
        }

        return Page();

    }

}
