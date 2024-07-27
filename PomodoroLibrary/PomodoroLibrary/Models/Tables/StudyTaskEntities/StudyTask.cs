using PomodoroLibrary.Models.Identity;
using PomodoroLibrary.Models.Tables.LabelEntities;
using PomodoroLibrary.Models.Tables.TaskPriorityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroLibrary.Models.Tables.StudyTaskEntities;

public class StudyTask
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public ApplicationUser User { get; set; }
    public string Name { get; set; }
    public bool Completed { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateCompleted { get; set; }
    public int? TaskPriorityId { get; set; }
    public TaskPriority? TaskPriority { get; set; }
    public bool Archived { get; set; }
    public DateTime? Deadline { get; set; }

    public List<TaskLabel> TaskLabels { get; } = [];
}
