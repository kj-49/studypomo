using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudyPomo.Library.Authorization;
using StudyPomo.Library.Models.Identity;
using StudyPomo.Library.Models.Tables.CourseEntities;
using StudyPomo.Library.Services.Interfaces;

namespace StudyPomo.UI.Pages.Manage.Courses;

public class AllModel : PageModel
{
    private readonly ICourseService _courseService;
    private readonly IUserService _userService;
    private readonly IAuthorizationService _authorizationService;

    public AllModel(ICourseService courseService, IUserService userService, IAuthorizationService authorizationService)
    {
        _courseService = courseService;
        _userService = userService;
        _authorizationService = authorizationService;
    }

    public CourseUpdate CourseUpdate { get; set; }
    public ICollection<Course> Courses { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        ApplicationUser user = await _userService.GetCurrentUserAsync(User);

        Courses = await _courseService.GetAllAsync(user.Id, includeArchived: true);

        return Page();
    }

    public async Task<IActionResult> OnPostArchiveAsync(int id)
    {
        ApplicationUser user = await _userService.GetCurrentUserAsync(User);

        await _courseService.ArchiveAsync(id);

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostActivateAsync(int id)
    {
        ApplicationUser user = await _userService.GetCurrentUserAsync(User);

        await _courseService.UnArchiveAsync(id);

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostUpdateCourseAsync(CourseUpdate courseUpdate)
    {
        Course course = await _courseService.GetAsync(courseUpdate.Id);

        var authResult = await _authorizationService.AuthorizeAsync(User, course, Operations.Update);

        if (!authResult.Succeeded)
        {
            if (User.Identity.IsAuthenticated)
            {
                return new ForbidResult();
            }
            else
            {
                return new ChallengeResult();
            }
        }

        await _courseService.UpdateAsync(courseUpdate);

        return RedirectToPage(new { id = courseUpdate.Id });
    }
}
