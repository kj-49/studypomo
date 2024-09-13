using Microsoft.AspNetCore.Mvc.Rendering;
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
        SelectList taskPriorities)
    {
        TimeZone = timeZone;
        StudyTaskUpdate = studyTaskUpdate;
        TaskLabels = taskLabels;
        TaskPriorities = taskPriorities;
    }

    public TimeZoneInfo TimeZone { get; set; }

    public StudyTaskUpdate StudyTaskUpdate { get; set; }
    public ICollection<TaskLabel> TaskLabels { get; set; }
    public SelectList TaskPriorities { get; set; }
}
