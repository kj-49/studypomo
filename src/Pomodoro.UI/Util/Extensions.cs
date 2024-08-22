using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace Pomodoro.UI.Util;

public static class Extensions
{
    public static SelectList ToSelectList(this IDictionary<string, string> map)
    {
        SelectList sl = new SelectList(map, "Key", "Value");
        return sl;
    }
}
