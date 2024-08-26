using Microsoft.AspNetCore.Mvc.Rendering;
using Pomodoro.Library.Models.Tables.LabelEntities;
using Pomodoro.Library.Models.Tables.StudyTaskEntities;
using Pomodoro.Library.Models.Tables.TaskPriorityEntities;

namespace Pomodoro.UI.ViewModels;

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
