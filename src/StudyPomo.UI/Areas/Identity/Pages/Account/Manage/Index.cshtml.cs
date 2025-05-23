using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Query.Internal;
using StudyPomo.Library.Migrations;
using StudyPomo.Library.Models.Identity;
using StudyPomo.Library.Models.Utility;
using StudyPomo.Library.Services;
using StudyPomo.Library.Services.Interfaces;
using StudyPomo.UI.Util.PageModels;
using System.ComponentModel.DataAnnotations;
using TimeZoneConverter;
using TimeZoneNames;

namespace StudyPomo.UI.Areas.Identity.Pages.Account.Manage;

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
        : base(userService)
    {
        _userService = userService;
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }

    public bool IsExternallyAuthenticated { get; set; }
    public SelectList TimeZones { get; set; }

    [BindProperty]
    public string? TimeZoneId { get; set; }
    [BindProperty]
    public bool SetTimeZoneAutomatically { get; set; }

    [BindProperty]
    public PasswordModel PasswordInput { get; set; }

    protected override async Task<TimeZoneInfo> ResolveTimeZone()
    {
        ApplicationUser? user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            throw new Exception("User not found.");
        }

        return TimeZoneInfo.FindSystemTimeZoneById(user.TimeZoneId ?? SD.UTC);
    }

    public async Task<IActionResult> OnGetAsync()
    {
        ApplicationUser user = await _userService.GetCurrentUserAsync();

        IsExternallyAuthenticated = await _userService.IsExternallyAuthenticated(user);

        TimeZones = TimeService.GetTimeZones().ToSelectList();

        TimeZoneId = user.TimeZoneId;

        SetTimeZoneAutomatically = user.SetTimeZoneAutomatically;

        return Page();
    }

    public async Task<IActionResult> OnPostSaveSettingsAsync()
    {
        ApplicationUser user = await _userService.GetCurrentUserAsync();

        user.SetTimeZoneAutomatically = SetTimeZoneAutomatically;

        if (!user.SetTimeZoneAutomatically)
        {
            user.TimeZoneId = TimeZoneId;
        }

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
