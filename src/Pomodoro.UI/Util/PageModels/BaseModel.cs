using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pomodoro.Library.Models.Identity;
using Pomodoro.Library.Services;
using Pomodoro.Library.Services.Interfaces;
using TimeZoneConverter;

namespace Pomodoro.UI.Util.PageModels;

public abstract class BaseModel : PageModel
{
    private readonly IUserService _userService;

    protected BaseModel(IUserService userService)
    {
        _userService = userService;
    }

    private TimeZoneInfo? _timeZone = null;

    public TimeZoneInfo TimeZone
    {
        get
        {
            if (_timeZone == null) throw new Exception("Timezone is not set. Ensure InitializeTimeZoneAsync has been called.");
            return _timeZone;
        }
        set => _timeZone = value;
    }

    protected abstract Task<TimeZoneInfo> ResolveTimeZone();

    public async Task InitializeTimeZoneAsync()
    {
        TimeZone = await ResolveTimeZone();
    }


    public async Task<IActionResult> OnPostUpdateTimeZoneAsync(string ianaTimeZone)
    {
        // Validate
        try
        {

            TimeZoneInfo tzi = TZConvert.GetTimeZoneInfo(ianaTimeZone);

            ApplicationUser user = await _userService.GetCurrentUserAsync(User);

            if (user == null) return NotFound();

            // If user has manually chosen their timezone, do not update automatically.
            if (!user.SetTimeZoneAutomatically)
            {
                return new OkResult();
            }

            user.TimeZoneId = tzi.Id;

            _userService.UpdateUser(user);

            return new OkResult();

        } catch (InvalidTimeZoneException ex)
        {
            // TODO: Log
            return NotFound();
        }
    }

}
