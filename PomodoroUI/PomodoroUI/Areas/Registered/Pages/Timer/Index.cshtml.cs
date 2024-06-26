
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PomodoroLibrary.Data.Interfaces;
using PomodoroLibrary.Models.Entities;
using PomodoroLibrary.Services;
using PomodoroLibrary.Services.Interfaces;

namespace PomodoroUI.Areas.Registered.Pages.Timer;

public class IndexModel : PageModel
{
    private readonly IUserService _userService;
    private readonly IStudyTaskRepository _taskRepository;

    public IndexModel(IUserService userService, IStudyTaskRepository taskRepository)
    {
        _userService = userService;
        _taskRepository = taskRepository;
    }

    public ICollection<StudyTask> Tasks { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        IdentityUser<int>? user = await _userService.GetCurrentUserAsync();

        Tasks = (await _taskRepository.GetAllAsync()).ToList();

        if (user == null) return RedirectToPage("/Pomodoro/Public/Index");

        return Page();
    }

    public async Task<IActionResult> OnPostCreateTaskAsync()
    {
        // Add task
        return RedirectToPage("./Index");
    }

}
