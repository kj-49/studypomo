using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pomodoro.Library.Models.Identity;
using Pomodoro.Library.Services.Interfaces;

namespace Pomodoro.UI.Areas.Identity.Pages.Account.Manage;

public class IndexModel : PageModel
{

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserService _userService;

    public IndexModel(UserManager<ApplicationUser> userManager, IUserService userService)
    {
        _userManager = userManager;
        _userService = userService;
    }

    public bool IsExternallyAuthenticated { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        ApplicationUser user = await _userService.GetCurrentUserAsync();

        IsExternallyAuthenticated = await _userService.IsExternallyAuthenticated(user);

        return Page();
    }
}
