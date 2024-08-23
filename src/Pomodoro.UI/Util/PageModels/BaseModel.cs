using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pomodoro.Library.Models.Identity;
using Pomodoro.Library.Services;
using Pomodoro.Library.Services.Interfaces;

namespace Pomodoro.UI.Util.PageModels;

public abstract class BaseModel : PageModel
{
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
}
