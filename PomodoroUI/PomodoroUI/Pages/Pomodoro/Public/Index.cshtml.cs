
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PomodoroLibrary.Models.Tables;
using PomodoroLibrary.Services.Interfaces;

namespace PomodoroUI.Pages.Pomodoro.Public;

public class IndexModel : PageModel
{
    private readonly ILoginService _loginService;
    private readonly IUserService _userService;

    public IndexModel(ILoginService loginService, IUserService userService)
    {
        _loginService = loginService;
        _userService = userService;
    }
    public async Task<IActionResult> OnGetAsync()
    {
        IdentityUser<int>? user = await _userService.GetCurrentUserAsync();

        if (user != null) return RedirectToPage("/Pomodoro/LoggedIn/Index");

        return Page();
    }
}
