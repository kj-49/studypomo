
using Htmx;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Framework;
using PomodoroLibrary.Data;
using PomodoroLibrary.Data.Interfaces;
using PomodoroLibrary.Models.Identity;
using PomodoroLibrary.Models.Tables.StudyTaskEntities;
using PomodoroLibrary.Models.Tables.TaskPriorityEntities;
using PomodoroLibrary.Services.Interfaces;
using System.Runtime.CompilerServices;

namespace PomodoroUI.Areas.Registered.Pages.Timer;

public class IndexModel : PageModel
{
    private readonly IUserService _userService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStudyTaskService _studyTaskService;

    public IndexModel(IUserService userService, IUnitOfWork unitOfWork, IStudyTaskService studyTaskService)
    {
        _userService = userService;
        _unitOfWork = unitOfWork;
        _studyTaskService = studyTaskService;
    }

    public ICollection<StudyTask> StudyTasks { get; set; }
    [BindProperty]
    public StudyTaskVM StudyTaskCreate { get; set; }
    [BindProperty]
    public StudyTaskVM StudyTaskEdit { get; set; }

    public bool RenderTasksOutOfBand { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        ApplicationUser? user = await _userService.GetCurrentUserAsync();

        StudyTasks = (await _unitOfWork.StudyTask.GetAllAsync()).ToList();

        StudyTaskCreate = new StudyTaskVM();
        StudyTaskCreate = new StudyTaskVM();

        if (user == null) return RedirectToPage("/Pomodoro/Public/Index");

        return Page();
    }

    public async Task<IActionResult> OnPostCreateStudyTaskAsync()
    {
        if (Request.IsHtmx()) {

            await _studyTaskService.CreateAsync(StudyTaskCreate);

            StudyTasks = (await _unitOfWork.StudyTask.GetAllAsync()).ToList();

            RenderTasksOutOfBand = true;

            StudyTaskCreate = new StudyTaskVM();

            return Partial("Partials/_StudyTasks", this);
        }

        return Page();
    }

    public async Task<IActionResult> OnPostRemoveStudyTaskAsync(int id)
    {
        if (Request.IsHtmx())
        {
            await _studyTaskService.RemoveAsync(id);

            StudyTasks = (await _unitOfWork.StudyTask.GetAllAsync()).ToList();

            RenderTasksOutOfBand = true;

            return Partial("Partials/_StudyTaskCreate", this);
        }

        return Page();
    }

    public async Task<IActionResult> OnGetStudyTaskAsync(int id)
    {
        return Page();
    }

}
