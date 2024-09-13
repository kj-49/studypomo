using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudyPomo.Library.Data.Database;
using System.Security.Claims;

namespace StudyPomo.UI.Util;

public static class Extensions
{
    public static SelectList ToSelectList(this IDictionary<string, string> map)
    {
        SelectList sl = new SelectList(map, "Key", "Value");
        return sl;
    }

    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        db.Database.Migrate();
    }
}
