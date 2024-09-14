using Microsoft.AspNetCore.Mvc.Rendering;
using StudyPomo.Library.Models.Tables.CourseEntities;
using StudyPomo.Library.Models.Tables.LabelEntities;
using StudyPomo.Library.Models.Tables.StudyTaskEntities;
using StudyPomo.Library.Models.Tables.TaskPriorityEntities;

namespace StudyPomo.UI.ViewModels;

public class EditStudyTaskVM
{
    public EditStudyTaskVM(
        TimeZoneInfo timeZone,
        StudyTaskUpdate studyTaskUpdate,
        ICollection<TaskLabel> taskLabels,
        SelectList taskPriorities,
        ICollection<Course> courses)
    {
        TimeZone = timeZone;
        StudyTaskUpdate = studyTaskUpdate;
        TaskLabels = taskLabels;
        TaskPriorities = taskPriorities;
        Courses = courses;
    }

    public TimeZoneInfo TimeZone { get; set; }

    public StudyTaskUpdate StudyTaskUpdate { get; set; }
    public ICollection<TaskLabel> TaskLabels { get; set; }
    public SelectList TaskPriorities { get; set; }
    public ICollection<Course> Courses { get; set; }
}
