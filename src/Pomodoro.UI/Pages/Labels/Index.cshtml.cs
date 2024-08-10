using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pomodoro.Library.Authorization;
using Pomodoro.Library.Models.Identity;
using Pomodoro.Library.Models.Tables.LabelEntities;
using Pomodoro.Library.Models.Tables.TaskLabelEntities;
using Pomodoro.Library.Services.Interfaces;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Pomodoro.UI.Pages.Labels;

public class IndexModel : PageModel
{
    private readonly ITaskLabelService _taskLabelService;
    private readonly IUserService _userService;
    private readonly IAuthorizationService _authorizationService;

    public IndexModel(ITaskLabelService taskLabelService, IUserService userService, IAuthorizationService authorizationService)
    {
        _taskLabelService = taskLabelService;
        _userService = userService;
        _authorizationService = authorizationService;
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

    public async Task<IActionResult> OnPostUpdateAsync(TaskLabelUpdate taskLabelUpdate)
    {
        TaskLabel? taskLabel = await _taskLabelService.GetAsync(taskLabelUpdate.Id);

        if (taskLabel == null) return NotFound();

        var authResult = await _authorizationService.AuthorizeAsync(User, taskLabel, Operations.Update);

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

        await _taskLabelService.UpdateAsync(taskLabelUpdate);

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostCreateAsync(TaskLabelCreate taskLabelCreate)
    {
        await _taskLabelService.CreateAsync(taskLabelCreate);

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        TaskLabel? taskLabel = await _taskLabelService.GetAsync(id);

        if (taskLabel == null) return NotFound();

        var authResult = await _authorizationService.AuthorizeAsync(User, taskLabel, Operations.Delete);

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

        await _taskLabelService.RemoveAsync(id);

        return RedirectToPage();
    }

}
