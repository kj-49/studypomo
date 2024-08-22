using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Pomodoro.Library.Migrations;
using Pomodoro.Library.Models.Identity;
using Pomodoro.Library.Models.Utility;
using Pomodoro.Library.Services;
using Pomodoro.Library.Services.Interfaces;
using Pomodoro.UI.Util.PageModels;
using System.ComponentModel.DataAnnotations;
using TimeZoneConverter;

namespace Pomodoro.UI.Areas.Identity.Pages.Account.Manage;

public class IndexModel : BaseModel
{
    private readonly IUserService _userService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(
        IUserService userService,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ILogger<IndexModel> logger)
    {
        _userService = userService;
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }

    public bool IsExternallyAuthenticated { get; set; }
    public SelectList TimeZones { get; set; }

    [BindProperty]
    public string? IanaTimeZone { get; set; }

    [BindProperty]
    public PasswordModel PasswordInput { get; set; }


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

    public async Task<IActionResult> OnPostChangePasswordAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        var changePasswordResult = await _userManager.ChangePasswordAsync(user, PasswordInput.OldPassword, PasswordInput.NewPassword);
        if (!changePasswordResult.Succeeded)
        {
            foreach (var error in changePasswordResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return Page();
        }

        await _signInManager.RefreshSignInAsync(user);

        return RedirectToPage();
    }

    public class PasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

}
