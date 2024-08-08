using Htmx;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PomodoroLibrary.Models.Identity;
using PomodoroLibrary.Models.Tables.CourseEntities;
using PomodoroLibrary.Models.Tables.LabelEntities;
using PomodoroLibrary.Models.Tables.StudyTaskEntities;
using PomodoroLibrary.Models.Tables.TaskPriorityEntities;
using PomodoroLibrary.Services.Interfaces;

namespace PomodoroUI.Pages.Manage.Courses;

public class IndexModel : PageModel
{
    private readonly ICourseService _courseService;
    private readonly ITaskPriorityService _taskPriorityService;
    private readonly ITaskLabelService _taskLabelService;
    private readonly IUserService _userService;
    private readonly IStudyTaskService _studyTaskService;

    public IndexModel(ICourseService courseService, ITaskPriorityService taskPriorityService, ITaskLabelService taskLabelService, IUserService userService, IStudyTaskService studyTaskService)
    {
        _courseService = courseService;
        _taskPriorityService = taskPriorityService;
        _taskLabelService = taskLabelService;
        _userService = userService;
        _studyTaskService = studyTaskService;
    }

    public Course Course { get; set; }

    [BindProperty]
    public StudyTaskCreate StudyTaskCreate { get; set; }
    public ICollection<TaskPriority> TaskPriorities { get; set; }
    public ICollection<TaskLabel> TaskLabels { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        ApplicationUser? user = await _userService.GetCurrentUserAsync();

        if (user == null) return Challenge();

        await PopulateFields(userId: user.Id, courseId: id);

        return Page();
    }

    public async Task PopulateFields(int userId, int courseId)
    {
        Course = await _courseService.GetAsync(courseId);
        TaskPriorities = await _taskPriorityService.GetAllAsync();
        TaskLabels = await _taskLabelService.GetAllAsync(userId);
    }

    public async Task<IActionResult> OnPostCreateStudyTaskAsync()
    {
        await _studyTaskService.CreateAsync(StudyTaskCreate);

        return RedirectToPage(new { id = StudyTaskCreate.CourseId });
    }

    public async Task<IActionResult> OnPostUpdateStudyTaskAsync(StudyTaskUpdate studyTaskUpdate)
    {
        await _studyTaskService.UpdateAsync(studyTaskUpdate);

        return RedirectToPage(new { id = studyTaskUpdate.CourseId });
    }

    public async Task<IActionResult> OnPostCompleteTaskAsync(int studyTaskId, int courseId)
    {
        if (Request.IsHtmx())
        {
            await _studyTaskService.CompleteAsync(studyTaskId);

            ApplicationUser? user = await _userService.GetCurrentUserAsync();

            if (user == null) return Challenge();

            await PopulateFields(userId: user.Id, courseId: courseId);

            return Partial("Partials/_Tasks", this);
        }

        return Page();
    }

    public async Task<IActionResult> OnPostUncompleteTaskAsync(int studyTaskId, int courseId)
    {
        if (Request.IsHtmx())
        {
            await _studyTaskService.UncompleteAsync(studyTaskId);

            ApplicationUser? user = await _userService.GetCurrentUserAsync();

            if (user == null) return Challenge();

            await PopulateFields(userId: user.Id, courseId: courseId);

            return Partial("Partials/_Tasks", this);
        }

        return Page();

    }

    public async Task<IActionResult> OnPostArchiveStudyTaskAsync(int studyTaskId, int courseId)
    {
        await _studyTaskService.ArchiveAsync(studyTaskId);

        ApplicationUser? user = await _userService.GetCurrentUserAsync();

        if (user == null) return Challenge();

        await PopulateFields(userId: user.Id, courseId: courseId);

        return RedirectToPage(new { id = courseId });
    }

}
