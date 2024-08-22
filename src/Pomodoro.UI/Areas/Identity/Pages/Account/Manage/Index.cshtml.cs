using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Pomodoro.Library.Models.Identity;
using Pomodoro.Library.Services;
using Pomodoro.Library.Services.Interfaces;
using Pomodoro.UI.Util.PageModels;
using TimeZoneConverter;

namespace Pomodoro.UI.Areas.Identity.Pages.Account.Manage;

public class IndexModel : BaseModel
{
    private readonly IUserService _userService;

    public IndexModel(IUserService userService, UserManager<ApplicationUser> userManager)
    {
        _userService = userService;
    }

    public bool IsExternallyAuthenticated { get; set; }
    public SelectList TimeZones { get; set; }

    [BindProperty]
    public string? IanaTimeZone { get; set; }


    public async Task<IActionResult> OnGetAsync()
    {
        ApplicationUser user = await _userService.GetCurrentUserAsync();

        IsExternallyAuthenticated = await _userService.IsExternallyAuthenticated(user);

        TimeZones = TimeService.GetIanaTimeZones().ToSelectList();

        IanaTimeZone = user.IanaTimeZone;

        return Page();
    }

    public async Task<IActionResult> OnPostSaveSettingsAsync()
    {
        ApplicationUser user = await _userService.GetCurrentUserAsync();

        user.IanaTimeZone = IanaTimeZone;
        user.TimeZoneChosen = true;

        _userService.UpdateUser(user);

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPageChangePasswordAsync()
    {
        return RedirectToPage();
    }
}
