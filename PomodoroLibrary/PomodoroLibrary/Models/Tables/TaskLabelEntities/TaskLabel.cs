using PomodoroLibrary.Models.Identity;
using PomodoroLibrary.Models.Tables.StudyTaskEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroLibrary.Models.Tables.LabelEntities;

public class TaskLabel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string HexColor { get; set; }
    public int UserId { get; set; }
    public ApplicationUser User { get; set; }

    public List<StudyTask> StudyTasks { get; set; } = [];
}
