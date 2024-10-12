using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudyPomo.Library.Models.Identity;
using StudyPomo.Library.Services;
using StudyPomo.Library.Services.Interfaces;
using System;
using System.Reflection.Metadata.Ecma335;

namespace StudyPomo.UI.Areas.Identity.Pages.Onboarding;

public class IndexModel : PageModel
{
    private readonly IUserService _userService;

    [BindProperty]
    public InputModel Input { get; set; }

    public SelectList TimeZones { get; set; }

    public IndexModel(IUserService userService)
    {
        _userService = userService;
    }

    public void OnGet()
    {
        TimeZones = TimeService.GetTimeZones().ToSelectList();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        ApplicationUser user = await _userService.GetCurrentUserAsync();

        user.TimeZoneId = Input.TimeZoneId;
        user.IsOnboarded = true;

        _userService.UpdateUser(user);

        return RedirectToPage("~", new { Area = "" });
    }

    public class InputModel
    {
        public string TimeZoneId { get; set; }
    }
}
