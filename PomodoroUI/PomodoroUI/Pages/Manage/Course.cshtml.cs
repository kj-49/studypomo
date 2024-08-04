using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PomodoroLibrary.Models.Tables.CourseEntities;
using PomodoroLibrary.Services.Interfaces;

namespace PomodoroUI.Pages.Manage;

public class CourseModel : PageModel
{
    private readonly ICourseService _courseService;

    public CourseModel(ICourseService courseService)
    {
        _courseService = courseService;
    }

    public Course Course { get; set; }

    public async Task OnGet(int id)
    {
        Course = await _courseService.GetAsync(id);   
    }
}
