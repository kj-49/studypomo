using PomodoroLibrary.Models.Tables.LabelEntities;
using PomodoroLibrary.Models.Tables.StudyTaskEntities;
using PomodoroLibrary.Models.Tables.TaskPriorityEntities;

namespace PomodoroUI.ViewModels;

public class EditStudyTaskVM
{
    public StudyTask StudyTask { get; set; }
    public StudyTaskUpdate StudyTaskUpdate { get; set; }
    public ICollection<TaskLabel> TaskLabels { get; set; }
    public ICollection<TaskPriority> TaskPriorities { get; set; }
}
