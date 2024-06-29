
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PomodoroLibrary.Models.Identity;
using PomodoroLibrary.Services.Interfaces;

namespace PomodoroUI.Areas.Public.Pages.Timer;

public class IndexModel : PageModel
{

    private readonly IUserService _userService;

    public IndexModel(IUserService userService)
    {
        _userService = userService;
    }
    public async Task<IActionResult> OnGetAsync()
    {
        ApplicationUser? user = await _userService.GetCurrentUserAsync();

        if (user != null) return RedirectToPage("/Pomodoro/LoggedIn/Index");

        return Page();
    }
}
