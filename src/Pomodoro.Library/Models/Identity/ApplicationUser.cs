using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodoro.Library.Models.Identity;

public class ApplicationUser : IdentityUser<int>
{
    public string? PreferredTheme { get; set; }
    public string? TimeZoneId { get; set; }
    public bool SetTimeZoneAutomatically { get; set; }
}
