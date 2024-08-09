using Pomodoro.Library.Models.Tables.LabelEntities;
using Pomodoro.Library.Models.Tables.StudyTaskEntities;
using Pomodoro.Library.Models.Tables.TaskPriorityEntities;

namespace Pomodoro.UI.ViewModels;

public class EditStudyTaskVM
{
    public StudyTask StudyTask { get; set; }
    public StudyTaskUpdate StudyTaskUpdate { get; set; }
    public ICollection<TaskLabel> TaskLabels { get; set; }
    public ICollection<TaskPriority> TaskPriorities { get; set; }
}
