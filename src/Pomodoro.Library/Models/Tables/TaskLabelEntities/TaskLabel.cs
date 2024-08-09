using Pomodoro.Library.Models.Identity;
using Pomodoro.Library.Models.Tables.StudyTaskEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodoro.Library.Models.Tables.LabelEntities;

public class TaskLabel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string HexColor { get; set; }
    public int UserId { get; set; }
    public ApplicationUser User { get; set; }

    public List<StudyTask> StudyTasks { get; set; } = [];
}
