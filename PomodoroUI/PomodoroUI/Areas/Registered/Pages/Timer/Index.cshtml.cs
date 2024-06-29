
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
    public StudyTaskCreate StudyTaskCreate { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        ApplicationUser? user = await _userService.GetCurrentUserAsync();

        StudyTasks = (await _unitOfWork.StudyTask.GetAllAsync()).ToList();
        StudyTaskCreate = new StudyTaskCreate();

        if (user == null) return RedirectToPage("/Pomodoro/Public/Index");

        return Page();
    }

    public async Task<IActionResult> OnPostCreateStudyTaskAsync()
    {
        // Add task
        await _studyTaskService.CreateAsync(StudyTaskCreate);

        return RedirectToPage();
    }

}
