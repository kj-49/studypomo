using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PomodoroLibrary.Models.Identity;
using PomodoroLibrary.Models.Tables.LabelEntities;
using PomodoroLibrary.Models.Tables.TaskLabelEntities;
using PomodoroLibrary.Services.Interfaces;
using System.Runtime.CompilerServices;

namespace PomodoroUI.Areas.Registered.Pages.Labels;

public class IndexModel : PageModel
{
    private readonly ITaskLabelService _taskLabelService;
    private readonly IUserService _userService;

    public IndexModel(ITaskLabelService taskLabelService, IUserService userService)
    {
        _taskLabelService = taskLabelService;
        _userService = userService;
    }

    public List<TaskLabel> TaskLabels { get; set; }
    public TaskLabelCreate TaskLabelCreate { get; set; }
    public TaskLabelUpdate TaskLabelUpdate { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        ApplicationUser? user = await _userService.GetCurrentUserAsync();
        if (user == null) return Challenge();

        TaskLabels = (await _taskLabelService.GetAllAsync(user.Id)).ToList();

        return Page();
    }

    public async Task OnPostUpdateAsync(TaskLabelUpdate taskLabelUpdate)
    {
        ApplicationUser? user = await _userService.GetCurrentUserAsync();
        if (user == null) return;

        await _taskLabelService.UpdateAsync(taskLabelUpdate);

        TaskLabels = (await _taskLabelService.GetAllAsync(user.Id)).ToList();
    }

    public async Task OnPostCreateAsync(TaskLabelCreate taskLabelCreate)
    {
        ApplicationUser? user = await _userService.GetCurrentUserAsync();
        if (user == null) return;

        await _taskLabelService.CreateAsync(taskLabelCreate);

        TaskLabels = (await _taskLabelService.GetAllAsync(user.Id)).ToList();
    }

    public async Task OnPostDeleteAsync(int id)
    {
        ApplicationUser? user = await _userService.GetCurrentUserAsync();

        if (user == null) return;

        await _taskLabelService.RemoveAsync(id);

        TaskLabels = (await _taskLabelService.GetAllAsync(user.Id)).ToList();
    }

}
